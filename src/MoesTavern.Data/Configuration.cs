using MoesTavern.Data.Data;
using MoesTavern.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace MoesTavern.Data
{
    public static class Configuration
    {
        public static IServiceCollection LoadDatabase(this IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<IInventoryRepository>(_ => new InventoryRepository(InventoryData.Cache(), InventoryData.Clients));
    }
}