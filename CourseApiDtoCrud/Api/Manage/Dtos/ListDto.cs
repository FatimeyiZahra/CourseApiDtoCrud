using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos
{
    public class ListDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalPage { get; set; }

    }
}
