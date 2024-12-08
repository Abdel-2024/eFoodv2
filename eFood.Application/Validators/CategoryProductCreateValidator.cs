using eFood.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.Validators
{
    public class CategoryProductCreateValidator: AbstractValidator<CategoryProductCreateDTO>
    {
        public CategoryProductCreateValidator()
        {
            RuleFor(x => x.CategoryProductName)
                .NotNull().WithMessage("Not null")
                .MinimumLength(3).WithMessage("Min 3")
                .MaximumLength(30).WithMessage("Max 10");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("Max 100");

            RuleFor(x => x.File)
                 .NotNull().WithMessage("Not null");
        }
    }
}
