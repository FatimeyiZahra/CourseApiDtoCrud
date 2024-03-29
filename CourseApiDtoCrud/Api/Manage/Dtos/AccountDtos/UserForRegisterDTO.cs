﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos.AccountDtos
{
    public class UserForRegisterDTO
    {

        [Required(ErrorMessage = "username is required")]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
       
        [Required]
        public string Password { get; set; }
    }
}
