namespace CinemaApp.Services.Data
{
    using Contracts;

    public class BaseService : IBaseService
    {
        public bool IsGuidValid(string? id, ref Guid guid)
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
