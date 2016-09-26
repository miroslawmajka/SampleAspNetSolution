using System.Web.Mvc;

namespace SampleAspNetMvcWebProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}