namespace Sportswear.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Configuration;
    using Sportswear.Models;

    public class ProductStaffController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly BlobController _blob;
        IHostingEnvironment _env;

        public ProductStaffController(ICosmosDbService cosmosDbService, IHostingEnvironment env)
        {
            _cosmosDbService = cosmosDbService;
            this._env = env;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetItemsAsync("SELECT * FROM c"));
        }

        [ActionName("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Id,Name,ImageURL,Price,Description,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddItemAsync(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,Name,ImageURL,Price,Description,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateItemAsync(product.Id, product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _cosmosDbService.GetItemAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _cosmosDbService.GetItemAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        {
            await _cosmosDbService.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetItemAsync(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToBlob(Product product)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _env.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.imgFile.FileName);
                string extension = Path.GetExtension(product.imgFile.FileName);
                string path = Path.Combine(wwwRootPath + "/Image", fileName);

                _blob.uploadFile(fileName, path);
                

            }
            return View(product);
        }
    }
}