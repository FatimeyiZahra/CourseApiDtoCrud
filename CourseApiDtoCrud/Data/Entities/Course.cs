using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Data.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }

        public Category Category { get; set; }
        public List<CourseTag> CourseTags { get; set; }

    }
}
