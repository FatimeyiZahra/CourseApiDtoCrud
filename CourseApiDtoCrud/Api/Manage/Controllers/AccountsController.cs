using CourseApiDtoCrud.Api.Manage.Dtos.AccountDtos;
using CourseApiDtoCrud.Data.Entities;
using CourseApiDtoCrud.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;


        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }



        //[HttpGet("test")]
        //public async Task<IActionResult> Test()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));

        //    AppUser appUser = new AppUser
        //    {
        //        UserName = "admin",
        //        FullName = "Zahra Sharifova"
        //    };
        //    await _userManager.CreateAsync(appUser, "admin123");
        //    await _userManager.AddToRoleAsync(appUser, "Admin");

        //    return NoContent();
        //}


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName=model.FullName
                
            };

            await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Member");

            return NoContent();
        }




        /// <summary>
        /// Api endpoint return  new access token
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///        "username": "admin1",
        ///        "password": admin123
        ///     }
        /// </remarks>
        /// <response code="401">Password incorrect</response>
        /// <response code="404">Admin not found</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginReturnDto))]
        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto loginDto)
        {
            AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null) return NotFound();

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized();

            string token = _jwtService.Generate(_userManager.GetRolesAsync(user).Result, user);

            LoginReturnDto loginReturnDto = new LoginReturnDto
            {
                UserName = user.UserName,
                Token = token
            };
            return Ok(token);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new
            {
                UserName = User.Identity.Name,
                FullName = User.FindFirst("FullName").Value
            });
        }



    }
}
