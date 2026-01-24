using ESozluk.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Validators
{
    public class AddTopicRequestValidator : AbstractValidator<AddTopicRequest>
    {
        public AddTopicRequestValidator() {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Topic adı boş bırakılamaz.");

            RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("Kullanıcı Id boş bırakılamaz.")
                    .GreaterThan(0).WithMessage("Geçerli bir user ID'si girilmelidir.");

            RuleFor(x => x.CategoryId)
                    .NotEmpty().WithMessage("Kategori Id boş bırakılamaz.")
                    .GreaterThan(0).WithMessage("Geçerli bir kategori ID'si girilmelidir.");
        }

    }
}
