using Data.Feedbacks;
using Data.Users;
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
        public static UserModel MapDomainToModel(User user, string token) =>
            new()
            {
                Id = user.Data.Id,
                Username = user.Data.Username,
                FirstName = user.Data.FirstName,
                LastName = user.Data.LastName,
                Token = token
            };
    }
}
