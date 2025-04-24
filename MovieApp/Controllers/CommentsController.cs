using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Controllers
{
    public class CommentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
