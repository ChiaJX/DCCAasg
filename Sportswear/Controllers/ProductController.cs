using Microsoft.AspNetCore.Mvc;
using Sportswear.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Sportswear.Controllers
{
    public class ProductController : Controller
    {

        int quantity = 1;
        private readonly ICosmosDbService _cosmosDbService;

        List<Product> cartItem = new List<Product>();

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

        async Task<Product> getProductByIdAsync(string id)
        {
            List<Product> productList = (await _cosmosDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return productList.Find(a => a.Id == id);
            
        }

        void addProductToCartList(Product product)
        {
            cartItem.Add(product);
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
