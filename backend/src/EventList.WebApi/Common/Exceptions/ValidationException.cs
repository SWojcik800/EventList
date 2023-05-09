using FluentValidation.Results;

namespace EventList.WebApi.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public string FormattedValidationErrors()
        {
            var validationMessages = new List<string>();

            foreach (var error in Errors)
            {
                validationMessages.Add($"{error.Key}: {string.Join("; ", error.Value)}");
            }

            return string.Join(", ", validationMessages);

        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
