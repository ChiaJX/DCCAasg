using Microsoft.AspNetCore.Mvc;
using Sportswear.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Diagnostics;
using Sportswear.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Sportswear.Areas.Identity.Data;

namespace Sportswear.Controllers
{
    public class ProductController : Controller
    {
        private readonly SportswearNewContext _context;
        private readonly ICosmosDbService _cosmosDbService;
        Product selectedProd;
        static volatile public string productID, productName;
        static volatile public string productPrice;
        static volatile public string userID;

        private readonly SignInManager<SportswearUser> _SignInManager;
        private readonly UserManager<SportswearUser> _UserManager;


        public ProductController(ICosmosDbService cosmosDbService, SportswearNewContext context,
            UserManager<SportswearUser> usrMgr, SignInManager<SportswearUser> signinMgr)
        {
            _cosmosDbService = cosmosDbService;
            _context = context;
            _UserManager = usrMgr;
            _SignInManager = signinMgr;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Product/{productId}")]
        public IActionResult productDetails(string productId)
        {
            ViewBag.quantity = "1";
            ViewBag.message = null;
            selectedProd = getProductByIdAsync(productId).Result;
            productID = productId;
            productName = selectedProd.Name;
            productPrice = selectedProd.Price.ToString();

            return View(selectedProd);
        }

        async Task<Product> getProductByIdAsync(string id)
        {
            List<Product> productList = (await _cosmosDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return productList.Find(a => a.Id == id);
        }



        //check if order exists? edit : create 
        public async Task<IActionResult> addToCart([Bind("transactionId,userId,userAddress,userPhone,orderId,product,couponId,message,price,TransactionDateTime,status")] Transaction transaction)
        {

            if (_SignInManager.IsSignedIn(User))
            {
                var user = from m in _UserManager.Users
                           where m.Id.Equals(_UserManager.GetUserId(User))
                           select m.Id;

                foreach (string Id in user)
                {
                    userID = Id;
                }
                transaction.userId = userID;

            } else
            {
                var rand = new Random();
                transaction.userId = rand.Next(100000, 1000000).ToString();
            }

            //gettransaction，need check also userId
            if (_context.Transaction.Count() > 0 || (_context.Transaction.Any(e => e.status == "unpaid")))
            {
                var getTransactionByStatus = await _context.Transaction.FirstOrDefaultAsync(m => m.status == "unpaid");
                try
                {
                    if (getTransactionByStatus.product == "")
                    {
                        transaction.product = productName;
                    }
                    else
                    {
                        transaction.product = getTransactionByStatus.product + "//" + productName;
                    }

                    transaction.price = getTransactionByStatus.price + decimal.Parse(productPrice);
                    transaction.TransactionDateTime = DateTime.Now;
                    transaction.userId = userID;
                    transaction.orderId = getTransactionByStatus.orderId;
                    transaction.userAddress = getTransactionByStatus.userAddress;
                    transaction.userPhone = getTransactionByStatus.userPhone;
                    transaction.couponId = getTransactionByStatus.couponId;
                    transaction.message = getTransactionByStatus.message;
                    transaction.status = getTransactionByStatus.status;
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction("Create", "Transactions", new { transactionId = transaction.transactionId, productId = productID });
            }
            else
            {
                createOrder(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Transactions", new { transactionId = transaction.transactionId, productId = productID });
            }
        }

        public void createOrder([Bind("transactionId,userId,userAddress,userPhone,orderId,product,couponId,message,price,TransactionDateTime,status")] Transaction transaction)
        {
            transaction.TransactionDateTime = DateTime.Now;
            transaction.orderId = transaction.TransactionDateTime + transaction.userId;
            transaction.userAddress = "0";
            transaction.userPhone = "0";
            transaction.product = productName;
            transaction.couponId = "0";
            transaction.message = "0";
            transaction.price = decimal.Parse(productPrice);
            transaction.status = "unpaid";
            _context.Add(transaction);
        }
    }
}
