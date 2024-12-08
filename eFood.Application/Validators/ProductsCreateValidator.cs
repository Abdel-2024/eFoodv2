using eFood.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.Validators
{
    public class ProductsCreateValidator : AbstractValidator<ProductsCreateDTO>
    {
        public ProductsCreateValidator()
        {
            
        }
    }
}
