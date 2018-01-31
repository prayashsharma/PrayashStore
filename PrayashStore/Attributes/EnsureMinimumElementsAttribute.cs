using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PrayashStore.Attributes
{
    public sealed class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        private string _errorMessage;
        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList;
            if (list != null)
            {
                if (list.Cast<object>().Count(x => x != null) >= _minElements)
                    return ValidationResult.Success;
            }

            _errorMessage = ErrorMessage == null
                ? $"Number of elements in {validationContext.MemberName.ToString()} has to be atleast {_minElements}"
                : ErrorMessage;

            return new ValidationResult(_errorMessage);
        }
    }
}
