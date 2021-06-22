using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Users
{
    public interface IUsersRepository
    {
        Task<AuthenticateResult> Authenticate(string username, string password);
        Task<List<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> Create(User user, string password);
        Task Update(User user, string password = null);
        Task Delete(int id);
    }
    public class AuthenticateResult
    {
        public static AuthenticateResult Success(User user)
        {
            return new() { User = user };
        }
        public static AuthenticateResult Failure(string errorMessage)
        {
            return new() { ErrorMessage = errorMessage };
        }

        public User? User { get; set; }
        public string? ErrorMessage { get; set; }
    }
}