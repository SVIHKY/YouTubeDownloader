using Microsoft.AspNetCore.Mvc;

namespace YouTubeDownloader.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
