using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos.AccountDtos
{
    public class LoginReturnDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
