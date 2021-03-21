using Microsoft.AspNetCore.Mvc;
using Sportswear.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Sportswear.Controllers
{
    public class ProductController : Controller
    {

        private readonly ICosmosDbService _cosmosDbService;
        Dictionary<Product, double> cartItem = new Dictionary<Product, double>() { };
        Product selectedProd;

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
            ViewBag.quantity = "1";
            ViewBag.productId = productId;
            ViewBag.message = null;
            selectedProd = getProductByIdAsync(productId).Result;
            return View(selectedProd);
        }

        async Task<Product> getProductByIdAsync(string id)
        {
            List<Product> productList = (await _cosmosDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return productList.Find(a => a.Id == id);
        }


        public IActionResult addQuantity(string quantity)
        {
            int qty = int.Parse(quantity);
            qty++;
            ViewBag.quantity = qty.ToString();
            return View(ViewBag.quantity);
        }

        void addToCartList()
        {
            cartItem.Add(selectedProd, selectedProd.Price);
            ViewBag.message = "Added to Cart.";
        }

        public Dictionary<Product, double> getCartItem()
        {
            return cartItem;
        }

        public IActionResult decQuantity(string quantity)
        {
            int qty = int.Parse(quantity);
            if (qty > 1)
            {
                qty--;
                ViewBag.quantity = qty.ToString();
            }
            return View(ViewBag.quantity);
        }
    }
}
