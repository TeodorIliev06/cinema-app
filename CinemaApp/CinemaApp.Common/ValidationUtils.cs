using System;
using System.Collections.Generic;
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
    }
}
