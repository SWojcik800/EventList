using EventList.WebApi.Common.Interfaces;

namespace EventList.WebApi.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}