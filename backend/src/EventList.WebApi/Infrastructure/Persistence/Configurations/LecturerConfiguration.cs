using EventList.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventList.WebApi.Infrastructure.Persistence.Configurations
{
    public class LecturerConfiguration : IEntityTypeConfiguration<Lecturer>
    {
        public void Configure(EntityTypeBuilder<Lecturer> builder)
        {
            builder.Ignore(e => e.DomainEvents);
        }
    }
}
