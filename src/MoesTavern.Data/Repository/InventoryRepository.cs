using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoesTavern.Data.Model;

namespace MoesTavern.Data.Repository
{
    public interface IInventoryRepository
    {
        Task<List<Beer>> All(Guid clientId);
        Task<Beer> Find(Guid clientId, int inventoryId);
        Task Add(Guid clientId, Beer item);
        Task Update(Guid clientId, int inventoryId, double barrelage);
        Task Remove(Guid clientId, int inventoryId);
        Task<bool> DoesClientExist(Guid clientId);
    }

    public class InventoryRepository : IInventoryRepository
    {
        private readonly Dictionary<string, Beer> _inventoryCache;
        private readonly List<Guid> _clients;

        public InventoryRepository(Dictionary<string, Beer> inventoryCache, List<Guid> clients)
        {
            _inventoryCache = inventoryCache;
            _clients = clients;
        }

        public async Task<Beer> Find(Guid clientId, int inventoryId)
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        public async Task<List<Beer>> All(Guid clientId)
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        public async Task Update(Guid clientId, int id, double barrelage)
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        public async Task Remove(Guid clientId, int inventoryId)
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        public async Task Add(Guid clientId, Beer item)
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        public async Task<bool> DoesClientExist(Guid clientId)
        {
            //TODO: implement
            throw new NotImplementedException();
        }
    }
}