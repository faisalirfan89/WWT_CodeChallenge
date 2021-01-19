using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoesTavern.Data.Model;
using System.Linq;
using Microsoft.AspNetCore.Http;

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
            var id = string.Concat(clientId, "-", inventoryId);
            var taskFindBeer = Task.Factory.StartNew(() =>
            {
                this._inventoryCache.TryGetValue(id, out Beer beer);
                return beer;
            });
            await taskFindBeer;
            return taskFindBeer.Result;
        }

        public async Task<List<Beer>> All(Guid clientId)
        {
            var taskAll = Task.Factory.StartNew(() =>
            {
                var ids = this._inventoryCache.Values.Select((b) => b.Id).ToList();
                var beerList = new List<Beer>();
                foreach (var id in ids)
                {
                    this._inventoryCache.TryGetValue(string.Concat(clientId, "-", id), out Beer beer);
                    if (beer != null)
                        beerList.Add(beer);
                }
                return beerList;
            });

            await taskAll;
            return taskAll.Result;
        }

        public async Task Update(Guid clientId, int id, double barrelage)
        {
            var beer = this.Find(clientId, id).Result;
            if (beer == null)
            {
                return;
            }

            var updateTask = Task.Factory.StartNew(() =>
            {
                beer.Barrelage = barrelage;
            });
            await updateTask;
        }

        public async Task Remove(Guid clientId, int inventoryId)
        {
            var taskRemoveBeer = Task.Factory.StartNew(() =>
            {
                this._inventoryCache.Remove(string.Concat(clientId, "-", inventoryId));
            });
            await taskRemoveBeer;
        }

        public async Task Add(Guid clientId, Beer item)
        {
            if (!this.DoesClientExist(clientId).Result)
                this._clients.Add(clientId);

            var taskAddBeer = Task.Factory.StartNew(() =>
            {
                this._inventoryCache.TryAdd(string.Concat(clientId, "-", item.Id), item);
            });
            await taskAddBeer;

        }

        public async Task<bool> DoesClientExist(Guid clientId)
        {
            var taskClientExist = Task.Factory.StartNew(() =>
            {

                return this._clients.Contains(clientId);
            });
            await taskClientExist;
            return taskClientExist.Result;
        }
    }
}