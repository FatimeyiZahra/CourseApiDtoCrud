using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos.CourseDto
{
    public class CourseCreateDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
    }
    public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotNull();
            RuleFor(x => x.Desc).MaximumLength(1500);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);  //must be 0 or more
            RuleFor(x => x.StartDate).GreaterThanOrEqualTo(DateTime.UtcNow.AddHours(4));
            RuleFor(x => x.CategoryId).NotNull();
        }
    }
}
