using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos.CategoriesDtos
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
    }
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().MaximumLength(50).WithMessage("Uzunluq 50-den boyuk ola bilmez");
        }
    }
}
