using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Users;
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

        public async Task<AuthenticateResult> Authenticate(string username, string password)
        {
            await using var session = _store.LightweightSession();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return AuthenticateResult.Failure("Wrong Password");

            var user = session.Query<UserData>().SingleOrDefault(x => x.Username == username);
            
            if (user == null)
                return AuthenticateResult.Failure("User not found");

            return !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)
                ? AuthenticateResult.Failure("Wrong password") : AuthenticateResult.Success(new User(user));
        }
        public async Task<List<User>> GetAll()
        {
            await using var session = _store.LightweightSession();
            return ToDomainObjectsList(await session.Query<UserData>().ToListAsync());
        }

        public async Task<User?> GetById(int id)
        {
            await using var session = _store.LightweightSession();

            var query = session.Query<UserData>();
            var user = query.FirstOrDefault(x => x.Id == id);
            return user == null ? null : ToDomainObject(user);
        }
        public async Task<User?> Create(User userRequest, string password)
        {
            await using var session = _store.LightweightSession();
            
            if (string.IsNullOrWhiteSpace(password))
                return null;

            if (session.Query<UserData>().Any(x => x.Username == userRequest.Data.Username))
                return null;

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
            await session.SaveChangesAsync();

            return ToDomainObject(userData);
        }

        public async Task Update(User userReq, string password)
        {
            await using var session = _store.LightweightSession();
            var user = session.Query<UserData>().First(x => x.Id == userReq.Data.Id);

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
            await session.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await using var session = _store.LightweightSession();
            
            var user = session.Query<UserData>().First(x => x.Id == id);
            
            session.Delete(user);
            await session.SaveChangesAsync();
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