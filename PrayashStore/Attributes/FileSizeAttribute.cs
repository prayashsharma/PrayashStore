using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace PrayashStore.Attributes
{
    public sealed class FileSizeAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "Please upload a file of less than {0} bytes";
        private readonly int? _maxBytes;

        public FileSizeAttribute(int maxBytes)
        {
            _maxBytes = maxBytes;
            if (ErrorMessage == null)
                ErrorMessage = string.Format(_defaultErrorMessage, _maxBytes.Value);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is HttpPostedFileBase)
            {
                var file = value as HttpPostedFileBase;

                if (file != null && file.ContentLength > _maxBytes.Value)
                    return new ValidationResult(ErrorMessageString);
            }

            if (value is IEnumerable<HttpPostedFileBase>)
            {
                var files = value as IEnumerable<HttpPostedFileBase>;

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > _maxBytes.Value)
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
                ValidationType = "filesize",
                ErrorMessage = ErrorMessageString
            };
            rule.ValidationParameters["maxbytes"] = _maxBytes;
            yield return rule;
        }
    }
}