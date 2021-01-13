using MoesTavern.Api.Model;
using MoesTavern.Data.Repository;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;

namespace MoesTavern.Api.Schema
{
    public interface IInventoryQuery : IObjectGraphType
    { }

    public class InventoryQuery : ObjectGraphType, IInventoryQuery
    {
        public InventoryQuery(IHttpContextAccessor contextAccessor, IInventoryRepository inventory)
        {
            FieldAsync<ListGraphType<BeerGraphType>>(
               name: "whatsOnTap",
               description: "Returns all beers in your inventory",
               resolve: async context =>
               {
                   //TODO: implement
                   throw new NotImplementedException();
               });

            FieldAsync<BeerGraphType>(
               name: "findBeer",
               description: "Finds the beer you're looking for",
               arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
               resolve: async context =>
               {
                   //TODO: implement
                   throw new NotImplementedException();
               });
        }
    }
}