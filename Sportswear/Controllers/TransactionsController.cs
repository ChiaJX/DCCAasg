using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sportswear.Controllers;
using Sportswear.Data;
using Sportswear.Models;

namespace Sportswear.Views.Transactions
{
    public class TransactionsController : Controller
    {
        private readonly SportswearNewContext _context;
        private readonly ICosmosDbService _cosmosDbService;
        Product prod;

        public TransactionsController(SportswearNewContext context, ICosmosDbService cosmosDbService)
        {
            _context = context;
            _cosmosDbService = cosmosDbService;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transaction.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.transactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        //Cart Page
        public async Task<IActionResult> Create(string transactionId, string msg)
        {
            if (transactionId == null)
            {
                var getTransactionByStatus = await _context.Transaction.FirstOrDefaultAsync(m => m.status == "unpaid");
                transactionId = getTransactionByStatus.transactionId.ToString();
            }

            List<Transaction> transactionList = _context.Transaction.ToList();
            List<Product> productList = new List<Product>();
            List<string> productNameList = new List<string>();
            List<int> productQtyList = new List<int>();
            ViewBag.prodNameList = productNameList;
            decimal TotalPrice = 0;

            foreach (var item in transactionList)
            {
                if (item.transactionId.ToString() == transactionId)
                {
                    Debug.WriteLine("prod name : " + item.product);
                    string[] pNameList = item.product.ToString().Split("//");

                    /*                    var qty = from name in pNameList
                                                    group name by name into gP
                                                    let count = gP.Count()
                                                    orderby count descending
                                                    select new { Value = gP.Key, Count = count };

                                        foreach (var name in qty)
                                        {
                                            productQtyList.Add(name.Count);
                                            Debug.WriteLine("Qty Result : " + productQtyList);
                                        }
                    */

                    foreach (var name in pNameList)
                    {
                        if (!productList.Contains(getProductByNameAsync(name).Result))
                        {
                            productList.Add(getProductByNameAsync(name).Result);
                            Debug.WriteLine("Result : " + getProductByNameAsync(name).Result);
                        }
                    }
                    TotalPrice = item.price;
                }
            }

            ViewBag.Products = productList;
            ViewBag.TotalPrice = TotalPrice;
            ViewBag.GrandTotalPrice = ViewBag.TotalPrice + 20;
            ViewBag.msg = msg;

            return View();

        }


        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("transactionId,userId,userAddress,userPhone,orderId,product,couponId,message,price,TransactionDateTime,status")] Transaction transaction)
        {
            if (id != transaction.transactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.transactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.transactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> deleteItem(string productName)
        {
            var getTransactionByStatus = await _context.Transaction.FirstOrDefaultAsync(m => m.status == "unpaid");
            List<string> productNameList = new List<string>();

            var transaction = await _context.Transaction.FindAsync(getTransactionByStatus.transactionId);

            Debug.WriteLine("prod name : " + productName);
            string[] pNameList = transaction.product.ToString().Split("//");

            foreach (var name in pNameList)
            {
                if (name.Equals(productName))
                {
                    pNameList = pNameList.Where(val => val != name).ToArray(); //recreate an array without the removed element
                }
            }

            return RedirectToAction("Create", new { msg = "Item deleted!" });
        }


        //GET: product by Name
        async Task<Product> getProductByNameAsync(string name)
        {
            List<Product> productList = (await _cosmosDbService.GetItemsAsync("SELECT * FROM c")).ToList();
            return productList.Find(a => a.Name == name);
        }

        //CHECK: transaction exists by id
        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.transactionId == id);
        }

        async Task<IActionResult> checkIdExists()
        {
            var getTransactionByStatus = await _context.Transaction.FirstOrDefaultAsync(m => m.status == "unpaid");
            Debug.WriteLine("run Check Id");
            return RedirectToAction("Create", new { transactionId = getTransactionByStatus.transactionId.ToString(), msg = "" });
        }
    }
}
