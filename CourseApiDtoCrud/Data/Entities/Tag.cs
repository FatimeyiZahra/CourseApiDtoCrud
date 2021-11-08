using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CourseTag> CourseTags { get; set; }
    }
}
