using Data.Feedbacks;
using Domain.Users;

namespace Facade.Users
{ 
    public static class UserMapper
    {
        public static User MapRequestToDomain(UserRequest userRequest) =>
            new(new UserData
            {
                Id = userRequest.Id, 
                Username=  userRequest.Username,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName
            });
    }
}
