using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
