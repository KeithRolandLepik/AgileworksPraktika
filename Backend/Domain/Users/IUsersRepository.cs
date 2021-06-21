using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Users
{
    public interface IUsersRepository
    {
        Task<User> Authenticate(string username, string password);
        Task<List<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> Create(User user, string password);
        Task Update(User user, string password = null);
        Task Delete(int id);
    }
}