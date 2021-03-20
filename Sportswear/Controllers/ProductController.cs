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
            ViewBag.quantity = quantity;
            return View();
        }

        public IActionResult productDetails(string productId)
        {
            ViewBag.productId = productId;
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

        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetItemAsync(id));
        }

        public IActionResult addToCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                //TODO: head to payment gateway
                return RedirectToPage(Payment);
            } else
            {
                return RedirectToRoute("/Account/Login");
            }
        }
    }
}
