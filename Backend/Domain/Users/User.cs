using Data.Feedbacks;
using Domain.Common;

namespace Domain.Users
{
    public class User : Entity<UserData>
    {
        public User() : this(null) { }
        public User(UserData data) : base(data) { }
    }
}
