using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Common
{
    public static class ValidationUtils
    {
        public static bool TryGetGuid(string? id, out Guid guid)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                guid = Guid.Empty;
                return false;
            }

            return Guid.TryParse(id, out guid);
        }

        public static bool IsValid(object obj)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            var context = new ValidationContext(obj);
            var isValid = Validator.TryValidateObject(obj, context, validationResults);

            return isValid;
        }
    }
}
