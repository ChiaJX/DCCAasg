using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sportswear.Controllers
{
    public class ProductController : Controller
    {

        int quantity = 1;
        private readonly ICosmosDbService _cosmosDbService;

        public ProductController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult productDetails(string productId)
        {
            ViewBag.productId = productId;
            return View();
        }

        Product getProductById(string id)
        {

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

        public IActionResult addToCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                //TODO: head to payment
                //return RedirectToPage(Payment);
                return RedirectToRoute("/Account/Login");
            } else
            {
                return RedirectToRoute("/Account/Login");
            }
        }
    }
}
