﻿using AutoMapper;
using CourseApiDtoCrud.Api.Manage.Dtos.CourseDto;
using CourseApiDtoCrud.Data;
using CourseApiDtoCrud.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CoursesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //-----------------------------GET-ALL-COURSE----------------------------------------https://localhost:44305/api/manage/courses

        [HttpGet]
        [Route("")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseListDto))]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            var courses = await _context.Courses.Include(x => x.Category).Skip((page - 1) * 10).Take(10).ToListAsync();

            CourseListDto courseList = new CourseListDto
            {
                Data = _mapper.Map<List<CourseListItemDto>>(courses),
                TotalPage = (int)Math.Ceiling(_context.Courses.Count() / 10d)
            };

            return Ok(courseList);
        }



        //-----------------------------GET-COURSE-BY-ID----------------------------------https://localhost:44305/api/manage/courses/1

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Course course = await _context.Courses.Include(x => x.CourseTags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == id);

            if (course == null) return NotFound();

            CourseDetailedDto courseDto = _mapper.Map<CourseDetailedDto>(course);


            return Ok(courseDto);
        }


        //-----------------------------CREATE-COURSE----------------------------------------https://localhost:44305/api/manage/courses

        [HttpPost("")]
        public async Task<IActionResult> Create(CourseCreateDto createDto)
        {
            if (!_context.Categories.Any(x => x.Id == createDto.CategoryId && !x.IsDeleted)) return NotFound("Category not found");

            Course course = _mapper.Map<Course>(createDto);

            if (createDto.TagIds != null && createDto.TagIds.Count > 0)
            {
                course.CourseTags = createDto.TagIds.Select(x => new CourseTag { TagId = x }).ToList();
            }

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { Id = course.Id });
        }


        //-----------------------------UPDATE-COURSE----------------------------------------https://localhost:44305/api/manage/courses/1

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CourseCreateDto createDto)
        {
            Course course = await _context.Courses.Include(x => x.CourseTags).FirstOrDefaultAsync(x => x.Id == id);

            if (course == null) return NotFound();

            if (!_context.Categories.Any(x => x.Id == createDto.CategoryId && !x.IsDeleted)) return NotFound();

            course.CategoryId = createDto.CategoryId;
            course.Name = createDto.Name;
            course.Price = createDto.Price;
            course.StartDate = createDto.StartDate;
            course.Desc = createDto.Desc;


            course.CourseTags.RemoveRange(0, course.CourseTags.Count);

            if (createDto.TagIds != null && createDto.TagIds.Count > 0)
            {
                course.CourseTags = createDto.TagIds.Select(x => new CourseTag { TagId = x }).ToList();
            }

            _context.SaveChanges();

            return NoContent();
        }


        //-----------------------------DELETE-COURSE----------------------------------------https://localhost:44305/api/manage/courses/1

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Course course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);

            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
