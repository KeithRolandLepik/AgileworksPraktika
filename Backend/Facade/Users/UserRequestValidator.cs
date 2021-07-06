using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Facade.Users
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Username).NotNull();
            RuleFor(x => x.Password).NotNull();
            RuleFor(x => x.Gender).NotNull().When(x => x.IsOtherGender);
         }
    }
}
