using CourseApiDtoCrud.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos.CourseDto
{
    public class CourseListDto
    {
        public List<CourseListItemDto> Data { get; set; }
        public int TotalPage { get; set; }
    }
}
