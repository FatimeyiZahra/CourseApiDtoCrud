using AutoMapper;
using CourseApiDtoCrud.Api.Manage.Dtos.CategoriesDtos;
using CourseApiDtoCrud.Data;
using CourseApiDtoCrud.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Route("api/manage/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //------------------------------GetAll--------------------------------------------https://localhost:44305/api/manage/categories
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(int page = 1)
        {
             var categories = await _context.Categories.Include(x=>x.Courses).Where(x => !x.IsDeleted).Skip((page - 1) * 10).Take(10).ToListAsync();
            CategoryListDto categoryList = new CategoryListDto
            {
                Data = _mapper.Map<List<CategoryListItemDto>>(categories),
                TotalPage = (int)Math.Ceiling(_context.Categories.Where(x=>!x.IsDeleted).Count() / 10d)
            };

            return Ok(categoryList);
        }


        //------------------------------GetById-----------------------------------------https://localhost:44305/api/manage/categories/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (category == null) return NotFound();

            CategoryDetailedDto detailedDto = _mapper.Map<CategoryDetailedDto>(category);

            return Ok(detailedDto);

        }



        //------------------------------Get-All-CATEGORY-WITHOUT-CATEGORY--------------https://localhost:44305/api/manage/categories/all

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Category> categories = _context.Categories.ToList();

            List<CategoryItemDto> categoryItems = _mapper.Map<List<CategoryItemDto>>(categories);

            return Ok(categoryItems);
        }


        //------------------------------CREATE--------------------------------------------https://localhost:44305/api/manage/categories

        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryCreateDto createDto)
        {
           if (_context.Categories.Any(x => x.Name.ToLower() == createDto.Name.ToLower() && !x.IsDeleted))
                return StatusCode(409); //this category already exist

            Category category = _mapper.Map<Category>(createDto);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { Id=category.Id});
        
        }


        //------------------------------UPDATE------------------------------------------https://localhost:44305/api/manage/categories/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryCreateDto updateDto)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (category == null) return NotFound();

            if (_context.Categories.Any(x => x.Name.ToLower() == updateDto.Name.ToLower() && x.Id != id && !x.IsDeleted)) //check is there any category with same name

                return StatusCode(409);

            category.Name = updateDto.Name;
            _context.SaveChanges();

            return NoContent();
        }


        //------------------------------Delete------------------------------------------https://localhost:44305/api/manage/categories/1

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (category == null) return NotFound();

            category.IsDeleted = true;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
