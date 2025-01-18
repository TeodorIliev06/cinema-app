namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Data.Models;
    using CinemaApp.Web.ViewModels.Cinema;
    using Microsoft.AspNetCore.Mvc;

    using Data;

    public class CinemaController(CinemaDbContext dbContext) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = dbContext
                .Cinemas
                .Select(c => new CinemaIndexViewModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location
                })
                .OrderBy(c => c.Location)
                .ToList();

            return View(cinemas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddCinemaFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var cinema = new Cinema()
            {
                Name = model.Name,
                Location = model.Location
            };

            dbContext.Cinemas.Add(cinema);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
