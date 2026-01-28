using ESozluk.Domain.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {

        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Kullanıcı adı en az 2 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress().WithMessage("Email boş bırakılamaz.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş bırakılamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
        }
    }
}
