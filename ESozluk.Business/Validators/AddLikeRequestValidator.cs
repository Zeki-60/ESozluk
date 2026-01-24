using ESozluk.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Validators
{
    public class AddLikeRequestValidator : AbstractValidator<AddLikeRequest>
    {
        public AddLikeRequestValidator() {
            RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("User Id boş bırakılamaz.");
            RuleFor(x => x.EntryId)
                    .NotEmpty().WithMessage("Entry Id boş bırakılamaz.")
                    .GreaterThan(0).WithMessage("Geçerli bir entry ID'si girilmelidir.");
        }
    }
}
