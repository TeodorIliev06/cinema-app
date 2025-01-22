namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Web.ViewModels.Movie;
    using Web.ViewModels.Cinema;

    public class CinemaController(CinemaDbContext dbContext) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = await dbContext
                .Cinemas
                .Select(c => new CinemaIndexViewModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location
                })
                .OrderBy(c => c.Location)
                .ToListAsync();

            return View(cinemas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCinemaFormModel model)
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

            await dbContext.Cinemas.AddAsync(cinema);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            var guid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guid);

            if (!isIdValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var cinema = await dbContext
                .Cinemas
                .Include(c => c.CinemaMovies)
                .ThenInclude(cm => cm.Movie)
                .FirstOrDefaultAsync(c => c.Id == guid);

            if (cinema == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new CinemaDetailsViewModel()
            {
                Name = cinema.Name,
                Location = cinema.Location,
                Movies = cinema.CinemaMovies
                    .Where(cm => cm.IsDeleted == false)
                    .Select(cm => new CinemaMovieViewModel()
                    {
                        Title = cm.Movie.Title,
                        Duration = cm.Movie.Duration,
                    })
                    .ToList()
            };

            return View(viewModel);
        }
    }
}
