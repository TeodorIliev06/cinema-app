using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.CinemaMovie
{
    public class AvailableTicketsViewModel
    {
        public string CinemaId { get; set; } = null!;
        public string MovieId { get; set; } = null!;
        public int AvailableTickets { get; set; }
        public int Quantity { get; set; }
    }
}
