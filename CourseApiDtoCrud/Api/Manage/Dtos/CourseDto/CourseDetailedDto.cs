﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Api.Manage.Dtos.CourseDto
{
    public class CourseDetailedDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TagInCourseDetailedDto> CourseTags { get; set; }
    }

    public class TagInCourseDetailedDto
    {
        public int TagId { get; set; }
        public string Name { get; set; }
    }
}
