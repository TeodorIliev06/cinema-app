using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.CinemaMovie
{
    public class GetAvailableTicketsFormModel
    {
        public string? CinemaId { get; set; }
        public string? MovieId { get; set; }
    }
}
