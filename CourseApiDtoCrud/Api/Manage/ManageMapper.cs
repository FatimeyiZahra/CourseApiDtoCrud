using AutoMapper;
using CourseApiDtoCrud.Api.Manage.Dtos.CategoriesDtos;
using CourseApiDtoCrud.Api.Manage.Dtos.CourseDto;
using CourseApiDtoCrud.Api.Manage.Dtos.TagDtos;
using CourseApiDtoCrud.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage
{
    public class ManageMapper :Profile
    {
        public ManageMapper()
        {
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryDetailedDto>();
            CreateMap<Category, CategoryListItemDto>();
            //.ForMember(dest => dest.Count, from => from.MapFrom(s => s.Courses.Count()));
            CreateMap<Category, CategoryInCourseListItemDto>().ForMember(dest => dest.Name, from => from.MapFrom(x => x.Name));
            CreateMap<Category, CategoryItemDto>();
            CreateMap<CourseCreateDto, Course>();
            CreateMap<Course, CourseListItemDto>();
            CreateMap<Course, CourseDetailedDto>();
            //.ForMember(dest => dest.Tags, from => from.MapFrom(x => x.CourseTags.Select(c => new TagInCourseDetailedDto { Id = c.TagId, Name = c.Tag.Name }).ToList()));
            CreateMap<CourseTag, TagInCourseDetailedDto>()
          .ForMember(dest => dest.Name, from => from.MapFrom(x => x.Tag.Name));

            CreateMap<TagCreateDto, Tag>();
            CreateMap<Tag, TagListItemDto>();
            CreateMap<Tag, TagDetailedDto>();
            CreateMap<Tag, TagItemDto>();
        }
    }
}
