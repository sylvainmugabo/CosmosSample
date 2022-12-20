using CosmosSample.Model;
using Microsoft.Azure.Cosmos;

namespace CosmosSample.Repository
{
    public class CosmosPostRepository : ICosmosDbRepository
    {
        private Container _container;

        public CosmosPostRepository(CosmosClient cosmosClient, string? databaseName, string? containerName)
        {

            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
        public async Task Addsync(Product product)
        {
            await _container.CreateItemAsync<Product>(product, new PartitionKey(product.categoryId));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<Product>(id, new PartitionKey(id));
        }

        public async Task<IEnumerable<Product>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Product>(new QueryDefinition(queryString));
            var results = new List<Product>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Product?> GetProductAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Product>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }

        public async Task UpdateAsync(string id, Product product)
        {
            await _container.UpsertItemAsync(product, new PartitionKey(id));
        }
    }
}