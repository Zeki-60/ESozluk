using ESozluk.Domain.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Validators
{
    public class AddCategoryRequestValidator : AbstractValidator<AddCategoryRequest>
    {
        public AddCategoryRequestValidator() {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Ad alanı boş bırakılamaz.");
}

    }
}
