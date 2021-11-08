using CourseApiDtoCrud.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Data.Configurations
{
    public class CourseTagConfiguration : IEntityTypeConfiguration<CourseTag>
    {
        public void Configure(EntityTypeBuilder<CourseTag> builder)
        {

        }
    }
}
