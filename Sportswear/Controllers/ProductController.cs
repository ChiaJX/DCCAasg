﻿using Microsoft.AspNetCore.Mvc;
using Sportswear.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Diagnostics;
using Sportswear.Data;
using Microsoft.EntityFrameworkCore;

namespace Sportswear.Controllers
{
    public class ProductController : Controller
    {
        private readonly SportswearNewContext _context;
        private readonly ICosmosDbService _cosmosDbService;
        Product selectedProd;
        static volatile public string productID, productName;
        static volatile public string productPrice;


        public ProductController(ICosmosDbService cosmosDbService, SportswearNewContext context)
        {
            _cosmosDbService = cosmosDbService;
            _context = context;
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


        /*        public void addToCart()
                {
                    Create();
                    ViewBag.message = "Added to Cart.";
                }
        */

        //check if order exists? edit : create 
        public async Task<IActionResult> addToCart([Bind("transactionId,userId,userAddress,userPhone,orderId,product,couponId,message,price,TransactionDateTime,status")] Transaction transaction)
        {
            //gettransaction，need check also userId
            if (_context.Transaction.Count() > 0 || (_context.Transaction.Any(e => e.status == "unpaid")))
            {
                var getTransactionByStatus = await _context.Transaction.FirstOrDefaultAsync(m => m.status == "unpaid");
                Debug.WriteLine("edit table2");
                transaction.product = getTransactionByStatus.product + "//" + productName;
                transaction.price = getTransactionByStatus.price + decimal.Parse(productPrice);
                transaction.TransactionDateTime = DateTime.Now;
                transaction.userId = getTransactionByStatus.userId;
                transaction.orderId = getTransactionByStatus.orderId;
                transaction.userAddress = getTransactionByStatus.userAddress;
                transaction.userPhone = getTransactionByStatus.userPhone;
                transaction.couponId = getTransactionByStatus.couponId;
                transaction.message = getTransactionByStatus.message;
                transaction.status = getTransactionByStatus.status;
                updateOrder(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Transactions", new { transactionId = transaction.transactionId});
            }
            else
            {
                createOrder(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Transactions", new { transactionId = transaction.transactionId });
            }
        }

        public void createOrder([Bind("transactionId,userId,userAddress,userPhone,orderId,product,couponId,message,price,TransactionDateTime,status")] Transaction transaction)
        {
            var rand = new Random();
            transaction.TransactionDateTime = DateTime.Now;
            transaction.userId = rand.Next(100000, 1000000).ToString();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void updateOrder([Bind("transactionId,userId,userAddress,userPhone,orderId,product,couponId,message,price,TransactionDateTime,status")] Transaction transaction)
        {
            _context.Update(transaction);
        }
    }
}
