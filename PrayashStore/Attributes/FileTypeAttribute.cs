using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrayashStore.Attributes
{
    public sealed class FileTypeAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "Only the following file types are allowed: {0}";
        private IEnumerable<string> _validTypes;

        public FileTypeAttribute(string validTypes)
        {
            _validTypes = validTypes.Split(',').Select(s => s.Trim().ToLower());

            if (ErrorMessage == null)
                ErrorMessage = string.Format(_defaultErrorMessage, string.Join(" or ", _validTypes));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is HttpPostedFileBase)
            {
                var file = value as HttpPostedFileBase;

                if (file != null && !_validTypes.Any(fileType => file.FileName.EndsWith(fileType)))
                    return new ValidationResult(ErrorMessageString);
            }

            if (value is IEnumerable<HttpPostedFileBase>)
            {
                var files = value as IEnumerable<HttpPostedFileBase>;

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file != null && !_validTypes.Any(fileType => file.FileName.EndsWith(fileType)))
                            return new ValidationResult(ErrorMessageString);
                    }
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "filetype",
                ErrorMessage = ErrorMessageString
            };
            rule.ValidationParameters.Add("validtypes", string.Join(",", _validTypes));
            yield return rule;
        }
    }
}