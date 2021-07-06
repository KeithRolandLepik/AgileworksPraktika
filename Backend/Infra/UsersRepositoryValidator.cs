using System.Linq;
using Data.Users;
using Domain.Users;
using Marten;

namespace Infra
{
    public static class UsersRepositoryValidator
    {
        public static (bool, string?) CanCreateUser(User user, IDocumentSession session)
        {
            return session.Query<UserData>().Any(x => x.Username == user.Data.Username) ? (true,null) : (false, "Cannot create user");
        }
    }
}
