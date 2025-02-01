using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Cinema
{
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;

    public class DeleteCinemaViewModel : IMapFrom<Cinema>
    {
        public string Id { get; set; } = null!;

        public string? Name { get; set; }

        public string? Location { get; set; }
    }
}
