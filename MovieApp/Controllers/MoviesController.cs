using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
