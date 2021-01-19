using System;
using MoesTavern.Api.Model;
using MoesTavern.Data.Repository;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using MoesTavern.Data.Model;
using GraphQL;
using System.Collections.Generic;

namespace MoesTavern.Api.Schema
{
    public interface IInventoryMutation : IObjectGraphType
    { }

    public class InventoryMutation : ObjectGraphType, IInventoryMutation
    {
        public InventoryMutation(IHttpContextAccessor contextAccessor, IInventoryRepository inventory)
        {
           
            FieldAsync<IntGraphType>(
               name: "deleteBeer",
               description: "Removes beer from inventory",
               arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
               resolve: async context =>
               {
                   if (this.IsUnAuthoried(contextAccessor))
                   {
                       throw new UnauthorizedAccessException();
                   }
                   await inventory.Remove(new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e7"), context.GetArgument<int>("id"));
                   return context.GetArgument<int>("id");
               });

            FieldAsync<BeerGraphType>(
               name: "addBeer",
               description: "Adds beer to inventory",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AddBeerGraphType>> { Name = "beer" }),
               resolve: async context =>
               {
                   if (this.IsUnAuthoried(contextAccessor))
                   {
                       throw new UnauthorizedAccessException();
                   }

                   await inventory.Add(new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e7"), context.GetArgument<Beer>("beer"));
                   return context.GetArgument<Beer>("beer");
               });

            FieldAsync<BeerGraphType>(
               name: "soldBeer",
               description: "Updates barrelage based on sold quantity",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SoldBeerGraphType>> { Name = "beer" }),
               resolve: async context =>
               {
                   if(this.IsUnAuthoried(contextAccessor))
                   {
                       throw new UnauthorizedAccessException();
                   }
                   var beer = context.GetArgument<SoldBeer>("beer");
                   await inventory.Update(new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e7"), beer.Id,beer.Quantity);
                   return await inventory.Find(new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e7"),beer.Id);
               });
        }

        private bool IsUnAuthoried(IHttpContextAccessor contextAccessor)
        {
            var unAuthrizationCode = new List<string>()
            {
                "Foo",
                "Bearer 12345asdf",
               $"Bearer {new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e9")}"
            };
            return (!contextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization") || unAuthrizationCode.Contains(contextAccessor.HttpContext.Request.Headers["Authorization"]));
        }

    }
}