using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos.CategoriesDtos
{
    public class CategoryListDto
    {
        public List<CategoryListItemDto> Data { get; set; }
        public int TotalPage { get; set; }
    }
}
