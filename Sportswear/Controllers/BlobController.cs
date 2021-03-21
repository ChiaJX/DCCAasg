using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sportswear.Controllers
{
    public class BlobController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
