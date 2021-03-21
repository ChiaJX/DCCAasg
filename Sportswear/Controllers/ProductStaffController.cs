namespace Sportswear.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
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
                uploadFile(product.Name, product.ImageURL);
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
            Product product = await _cosmosDbService.GetItemAsync(id);
            delete(product.Name);
            await _cosmosDbService.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetItemAsync(id));
        }


        private CloudBlobContainer getBlobStorageInformation()
        {
            //read json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();

            //get the connectionstring 
            CloudStorageAccount accountObject = CloudStorageAccount.Parse(configure["ConnectionStrings:SportswearBlobStorage"]);

            //refer container name
            CloudBlobClient clientAgent = accountObject.CreateCloudBlobClient();
            CloudBlobContainer container = clientAgent.GetContainerReference("productblob");

            return container;
        }

        public Boolean uploadFile(string imgName, string imgLocation)
        {
            CloudBlobContainer container = getBlobStorageInformation();
            CloudBlockBlob blob = container.GetBlockBlobReference(imgName);

            using (var fileStream = System.IO.File.OpenRead(imgLocation))
            {
                blob.UploadFromStreamAsync(fileStream).Wait();
            }
            return true;
        }



        public ActionResult listing(string message = null)
        {
            ViewBag.msg = message;
            CloudBlobContainer container = getBlobStorageInformation();

            //create a new empty list to contain the blobs information
            List<string> blobitems = new List<string>();

            BlobResultSegment result = container.ListBlobsSegmentedAsync(null).Result;

            //split the items in the result list one by one
            foreach (IListBlobItem item in result.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob)) //filter the blob type
                {
                    CloudBlockBlob singleblob = (CloudBlockBlob)item;
                    //block blob = video / audio / images (jpg/png/gif)
                    if (Path.GetExtension(singleblob.Name.ToString()) == ".jpg")
                    {
                        //add the item info to the list<string>
                        blobitems.Add(singleblob.Name + "#" + singleblob.Uri.ToString());
                    }
                }
            }
            return View(blobitems);
        }


        public ActionResult delete(string imgName)
        {
            CloudBlobContainer container = getBlobStorageInformation();
            string blobname = ""; string messageContent = "";
            try
            {
                //find item based on name inside the blob storage
                CloudBlockBlob blobitem = container.GetBlockBlobReference(imgName);
                blobname = blobitem.Name;

                //delete the item once found
                blobitem.DeleteIfExistsAsync();
                messageContent = blobname + " is successfully deleted from the blob storage.";
            }
            catch (Exception ex)
            {
                messageContent = "Tech issue: " + ex.ToString() + ". Please try again!";
            }
            return RedirectToAction("listing", "Blobs", new { message = messageContent });
        }
    }
}