using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos.CourseDto
{
    public class CourseListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int CategoryId { get; set; }
        //public string CategoryName { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public CategoryInCourseListItemDto Category { get; set; }
    }

    public class CategoryInCourseListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
