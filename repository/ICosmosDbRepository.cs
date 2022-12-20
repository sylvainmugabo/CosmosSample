using CosmosSample.Model;

namespace CosmosSample.Repository
{
    public interface ICosmosDbRepository
    {
        Task<IEnumerable<Product>> GetMultipleAsync(string query);
        Task<Product?> GetProductAsync(string id);
        Task Addsync(Product product);
        Task UpdateAsync(string id, Product product);
        Task DeleteAsync(string id);
    }
}