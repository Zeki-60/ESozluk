using ESozluk.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Validators
{
    public class AddEntryRequestValidator : AbstractValidator<AddEntryRequest>
    {
        public AddEntryRequestValidator() {
            RuleFor(x => x.Name)
                        .NotEmpty().WithMessage("Entry adı boş bırakılamaz.");

            RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Açıklama boş bırakılamaz.");

            RuleFor(x => x.TopicId)
                    .NotEmpty().WithMessage("Topic Id boş bırakılamaz.")
                    .GreaterThan(0).WithMessage("Geçerli bir topic ID'si girilmelidir.");

            

        }
    }
}
