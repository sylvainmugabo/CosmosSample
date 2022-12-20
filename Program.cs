using CosmosSample.Model;
using CosmosSample.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var vaultName = "your key vault url";

var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .AddAzureKeyVault(vaultName)
        .Build();
var builder = new ServiceCollection().AddSingleton<ICosmosDbRepository>(await InitAsync(config));
var app = builder.BuildServiceProvider();
var dbService = app.GetRequiredService<ICosmosDbRepository>();

//1) Add product 
Product newItem = new(
    id: Guid.NewGuid().ToString(),
    categoryId: Guid.NewGuid().ToString(),
    categoryName: "new product",
    name: "test",
    quantity: 12,
    sale: false
);

await dbService.Addsync(newItem);

//2) query database for all product 
var products = await dbService.GetMultipleAsync("SELECT * FROM p");
foreach (var product in products)
{
    System.Console.WriteLine(product.name + ", " + product.sale);
}

async Task<CosmosPostRepository> InitAsync(IConfiguration config)
{
    var dbName = config["CosmosDb:DatabaseName"] ?? throw new ArgumentNullException();
    var container = config["CosmosDb:ContainerName"] ?? throw new ArgumentNullException();
    var account = config["cosmodb-url"];
    var key = config["cosmosdb-key"];

    var client = new CosmosClient(account, key);
    var database = await client.CreateDatabaseIfNotExistsAsync(dbName);
    await database.Database.CreateContainerIfNotExistsAsync(container, "/categoryId");
    var cosmosDbService = new CosmosPostRepository(client, dbName, container);

    return cosmosDbService;
}


