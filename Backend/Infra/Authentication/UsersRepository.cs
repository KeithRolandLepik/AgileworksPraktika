using System;
using System.Collections.Generic;
using System.Linq;
using Data.Feedbacks;
using Domain.Users;
using Marten;

namespace Infra.Authentication
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDocumentStore _store;
        
        public UsersRepository(IDocumentStore store)
        {
            _store = store;
        }

        public User Authenticate(string username, string password)
        {
            using var session = _store.LightweightSession();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = session.Query<UserData>().SingleOrDefault(x => x.Username == username);
            
            if (user == null)
                return null;

            return !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null : new User(user);
        }

        public IEnumerable<User> GetAll()
        {
            using var session = _store.LightweightSession();
            return ToDomainObjectsList(session.Query<UserData>());
        }

        public User GetById(int id)
        {
            using var session = _store.LightweightSession();
            return ToDomainObject(session.Query<UserData>().First(x => x.Id == id));
        }

        public User Create(User userRequest, string password)
        {
            using var session = _store.LightweightSession();
            
            if (string.IsNullOrWhiteSpace(password))
                return new User();

            if (session.Query<UserData>().Any(x => x.Username == userRequest.Data.Username))
                return new User();

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            var userData = new UserData
            {
                FirstName = userRequest.Data.FirstName,
                LastName = userRequest.Data.LastName,
                Username = userRequest.Data.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            session.Store(userData);
            session.SaveChanges();

            return ToDomainObject(userData);
        }

        public void Update(User userReq, string password = null)
        {
            using var session = _store.LightweightSession();
            var user = session.Query<UserData>().First(x => x.Id == userReq.Data.Id);

            if (user == null)
                return;
            
            if (!string.IsNullOrWhiteSpace(userReq.Data.Username) && userReq.Data.Username != user.Username)
            {
                if (session.Query<UserData>().Any(x => x.Username == userReq.Data.Username))
                    return;

                user.Username = userReq.Data.Username;
            }
            
            if (!string.IsNullOrWhiteSpace(userReq.Data.FirstName))
                user.FirstName = userReq.Data.FirstName;

            if (!string.IsNullOrWhiteSpace(userReq.Data.LastName))
                user.LastName = userReq.Data.LastName;
            
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            session.Update(user);
            session.SaveChanges();
        }

        public void Delete(int id)
        {
            using var session = _store.LightweightSession();
            
            var user = session.Query<UserData>().First(x => x.Id == id);

            if (user == null) return;
            
            session.Delete(user);
            session.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, IReadOnlyList<byte> storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Count != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != storedHash[i]).Any();
        }
        internal List<User> ToDomainObjectsList(IEnumerable<UserData> set) => set.Select(ToDomainObject).ToList();

        protected internal User ToDomainObject(UserData entityData) => new(entityData);
    }
}