using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SubscriptionPlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
