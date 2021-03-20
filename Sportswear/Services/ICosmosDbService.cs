namespace Sportswear
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sportswear.Models;

    public interface ICosmosDbService
    {
        Task<IEnumerable<Product>> GetItemsAsync(string query);
        Task<Product> GetItemAsync(string id);
        Task AddItemAsync(Product product);
        Task UpdateItemAsync(string id, Product product);
        Task DeleteItemAsync(string id);
    }
}