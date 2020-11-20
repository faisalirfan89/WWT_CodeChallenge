using System;
using MoesTavern.Api.Model;
using MoesTavern.Data.Repository;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;

namespace MoesTavern.Api.Schema
{
    public interface IInventoryMutation : IObjectGraphType 
    {}
    
    public class InventoryMutation : ObjectGraphType, IInventoryMutation
    {
        public InventoryMutation(IHttpContextAccessor contextAccessor, IInventoryRepository inventory)
        {
            FieldAsync<IntGraphType>(
               name: "deleteBeer",
               description: "Removes beer from inventory",
               arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
               resolve: async context => {
                   //TODO: implement
                   throw new NotImplementedException();
                });
            
            FieldAsync<BeerGraphType>(
               name: "addBeer",
               description: "Adds beer to inventory",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AddBeerGraphType>> { Name = "beer" } ),
               resolve: async context => {
                   //TODO: implement
                   throw new NotImplementedException();
                });

            FieldAsync<BeerGraphType>(
               name: "soldBeer",
               description: "Updates barrelage based on sold quantity",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SoldBeerGraphType>> { Name = "beer" } ),
               resolve: async context => {
                   //TODO: implement
                   throw new NotImplementedException();
                });
        }
    }
}