using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Common
{
    public static class ValidationUtils
    {
        public static bool IsGuidValid(string? id, ref Guid guid)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            bool isGuidValid = Guid.TryParse(id, out guid);

            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }
    }
}
