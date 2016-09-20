using System.Web.Mvc;

namespace SampleVideoStreamingSite.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {
            return View();
        }
    }
}