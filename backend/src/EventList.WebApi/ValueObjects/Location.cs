using EventList.WebApi.Application.Common;

namespace EventList.WebApi.ValueObjects
{
    public class Location : ValueObject
    {
        public string Name { get; private set; } = "Site";
        public string? Country { get; private set; }
        public string? City { get; private set; }
        public string? Street { get; private set; }


        static Location() { }
        public Location() { }

        public Location(string name, string country, string city, string street)
        {
            Name = name;
            Country = country;
            City = city;
            Street = street;
        }

        public override string ToString()
        {
            var result = $"{Country}, {City} {Street}: {Name}";
            return result;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Country;
            yield return City;
            yield return Street;

        }
    }
}
