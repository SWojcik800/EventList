using EventList.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventList.WebApi.Infrastructure.Persistence.Configurations
{
    public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {

            builder.Ignore(e => e.DomainEvents);

            builder
                .OwnsOne(b => b.Location);
        }
    }
}
