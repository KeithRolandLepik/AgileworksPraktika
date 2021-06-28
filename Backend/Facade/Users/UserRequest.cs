using Data.Common;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Facade.Users
{
    public class UserRequest : UniqueEntityData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public bool IsOtherGender { get; set; }
    }
}