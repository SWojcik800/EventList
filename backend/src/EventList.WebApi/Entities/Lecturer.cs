using EventList.WebApi.Common;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace EventList.WebApi.Entities
{
    public sealed class Lecturer : AuditableEntity
    {
        private Lecturer()
        {
            //For EF Core 
        }

        private static readonly Regex _whitespaceRegex = new Regex(@"\s+");
        private string _name;

        public Lecturer(string? name, string? description)
        {
            Name = name;
            Description = description;
        }

        public int Id { get; set; }

        public IList<Lecture> Lectures { get; private set; }

        public string? Name { 
            get => _name; 
            set
            {
                _name = NormalizeName(value);
            } 
        }

        public string? Description { get; set; }

        public void UpdateLectures(IList<Lecture> lectures)
        {
            Lectures = lectures;
        }

        private string NormalizeName(string inputName)
        {
            var normalizedName = _whitespaceRegex.Replace(inputName.Trim(), " ");
            return normalizedName;
        }
    }
}
