using System;
using System.Collections.Generic;
using MoesTavern.Data.Model;

namespace MoesTavern.Data.Data
{
    public static class InventoryData
    {
        public static Guid DuffBreweryClientId = new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e7");

        public static List<Guid> Clients { get; } = new List<Guid>();

        private static IDictionary<string, Beer> _inventoryData { get; } = new Dictionary<string, Beer>();

        public static Dictionary<string, Beer> Cache()
        {
            if(!Clients.Contains(DuffBreweryClientId))
                Clients.Add(DuffBreweryClientId);

            foreach (var client in Clients)
            {
                Add(client, 1, 100, "Duff", "Lager");
                Add(client, 2, 50, "Duff Light", "Lager");
                Add(client, 3, 10, "Duff Dry", "Lager");
                Add(client, 4, 30, "Duff Stout", "Stout");
                Add(client, 5, 30, "Raseberry Duff", "IPA");
                Add(client, 6, 30, "Pawtucket Patriot Ale", "Pale Ale");
            }
            return _inventoryData as Dictionary<string, Beer>;
        }
        
        private static void Add(Guid clientId, int id, int barrels, string name, string style) =>
            _inventoryData.TryAdd( string.Concat(clientId, "-", id), 
                                            new Beer 
                                                    { 
                                                        Id = id,
                                                        Barrelage = barrels, 
                                                        Name = name, 
                                                        Style = style
                                                    }
                );
    }
}