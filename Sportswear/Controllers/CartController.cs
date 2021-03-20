using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Sportswear.Controllers
{

    public class CartController : Controller
    {

        ArrayList cartList = new ArrayList();


        public IActionResult Index()
        {
            return View();
        }

        public ActionResult GetSelectedProduct(string productName)
        {
            ViewBag.Message = productName;
            return View();
        }

    }
}
