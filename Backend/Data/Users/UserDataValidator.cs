using FluentValidation;

namespace Data.Users
{
    public class UserDataValidator : AbstractValidator<UserData>
    {
        public UserDataValidator()
        {
            RuleFor(user => user.Username).NotNull();
        }
    }
}
