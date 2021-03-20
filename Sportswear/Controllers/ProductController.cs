using Microsoft.AspNetCore.Mvc;

namespace Sportswear.Controllers
{
    public class ProductController : Controller
    {

        int quantity = 1;

        public IActionResult Index()
        {
            ViewBag.quantity = quantity;
            return View();
        }

        public IActionResult productDetails(string productName)
        {
            ViewBag.productName = productName;
            return View();
        }

        public IActionResult addQuantity(string productName)
        {
            ViewBag.productName = productName;
            return View();
        }

        public IActionResult manageQuantity(string function)
        {
            if(function == "add")
            {
                quantity++;
                ViewBag.quantity = quantity;
            } else
            {
                if(quantity > 1)
                {
                    quantity--;
                    ViewBag.quantity = quantity;
                }
            }
            return View();
        }
    }
}
