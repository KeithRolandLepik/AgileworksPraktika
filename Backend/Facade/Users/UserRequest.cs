using Data.Common;

namespace Facade.Users
{
    public class UserRequest : UniqueEntityData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}