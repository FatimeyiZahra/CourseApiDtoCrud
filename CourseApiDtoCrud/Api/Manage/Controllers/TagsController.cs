using AutoMapper;
using CourseApiDtoCrud.Api.Manage.Dtos.TagDtos;
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
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TagsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //-----------------------------CREATE-TAG---------------------------------------------https://localhost:44305/api/manage/tags

        [HttpPost("")]
        public async Task<IActionResult> Create(TagCreateDto createDto)
        {
            if (await _context.Tags.AnyAsync(x => x.Name.ToLower() == createDto.Name.ToLower()))
                return Conflict();

            Tag tag = _mapper.Map<Tag>(createDto);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { Id = tag.Id });
        }



        //-----------------------------UPDATE-TAG-------------------------------------------https://localhost:44305/api/manage/tags/1

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TagCreateDto createDto)
        {
            Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag == null) return NotFound();

            if (await _context.Tags.AnyAsync(x => x.Id != id && x.Name.ToLower() == createDto.Name.ToLower()))
                return Conflict();

            tag.Name = createDto.Name;

            await _context.SaveChangesAsync();

            return NoContent();
        }



        //-----------------------------GET-TAG-BY-ID----------------------------------------https://localhost:44305/api/manage/tags/1

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag == null) return NotFound();

            TagDetailedDto tagDto = _mapper.Map<TagDetailedDto>(tag);

            return Ok(tagDto);
        }



        //-----------------------------DELETE-TAG-------------------------------------------https://localhost:44305/api/manage/tags/1

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag == null) return NotFound();

            _context.Tags.Remove(tag);
            _context.SaveChanges();

            return NoContent();
        }




        //-----------------------------GET-ALL-TAGS-------------------------------------------https://localhost:44305/api/manage/tags

        [HttpGet("")]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Tag> tags = await _context.Tags.Skip((page - 1) * 10).Take(10).ToListAsync();

            TagListDto tagList = new TagListDto
            {
                TotalPage = (int)Math.Ceiling(_context.Tags.Count() / 10d),
                Data = _mapper.Map<List<TagListItemDto>>(tags)
            };


            return Ok(tagList);
        }



        //-----------------------------GET-ALL-TAGS-WITHOUT-PAGES-----------------------------------https://localhost:44305/api/manage/tags/all

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();

            List<TagItemDto> tagItems = _mapper.Map<List<TagItemDto>>(tags);

            return Ok(tagItems);
        }
    }
}
