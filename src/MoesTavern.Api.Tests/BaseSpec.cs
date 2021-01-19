using System;
using System.Threading.Tasks;
using GraphQL;
using Xunit;
using Microsoft.AspNetCore.Http;
using GraphQL.Types;

namespace MoesTavern.Api.Tests
{
    public class BaseSpec
    {
        protected async Task ThrowsUnauthorizedAccessException(FieldType field, IHttpContextAccessor accessor)
        {
            accessor.HttpContext.Request.Headers.Clear();
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await (Task<object>)field.Resolver.Resolve(new ResolveFieldContext())
                );

            accessor.HttpContext.Request.Headers["Authorization"] = "Foo";
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await (Task<object>)field.Resolver.Resolve(new ResolveFieldContext())
                );

            accessor.HttpContext.Request.Headers["Authorization"] = "Bearer 12345asdf";
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await (Task<object>)field.Resolver.Resolve(new ResolveFieldContext())
                );

            accessor.HttpContext.Request.Headers["Authorization"] = $"Bearer {new Guid("87c4705d-f4fe-489f-b4d3-dae2c774c2e9")}";
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await (Task<object>)field.Resolver.Resolve(new ResolveFieldContext())
                );
        }
    }
}