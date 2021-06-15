using Data.Common;

namespace Facade.Users
{
    public class UserModel: UniqueEntityData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}
