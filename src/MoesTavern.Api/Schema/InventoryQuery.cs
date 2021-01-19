using MoesTavern.Api.Model;
using MoesTavern.Data.Repository;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using GraphQL;

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
                   if (this.IsUnAuthoried(contextAccessor))
                   {
                       throw new UnauthorizedAccessException();
                   }

                   return await inventory.All(new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e7"));
              
               });

            FieldAsync<BeerGraphType>(
               name: "findBeer",
               description: "Finds the beer you're looking for",
               arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
               resolve: async context =>
               {
                   if (this.IsUnAuthoried(contextAccessor))
                   {
                       throw new UnauthorizedAccessException();
                   }

                   return await inventory.Find(new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e7"),context.GetArgument<int>("id"));
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