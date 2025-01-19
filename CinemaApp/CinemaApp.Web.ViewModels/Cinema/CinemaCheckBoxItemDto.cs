using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Cinema
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Cinema;
    public class CinemaCheckBoxItemDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(LocationMinLength)]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; } = null!;

        public bool IsSelected { get; set; }
    }
}
