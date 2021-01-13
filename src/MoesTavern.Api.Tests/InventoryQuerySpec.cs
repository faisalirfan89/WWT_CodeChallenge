using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoesTavern.Api.Schema;
using MoesTavern.Data.Data;
using MoesTavern.Data.Model;
using MoesTavern.Data.Repository;
using GraphQL;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using GraphQL.Types;

namespace MoesTavern.Api.Tests
{
    public class InventoryQuerySpec : BaseSpec
    {
        private readonly InventoryQuery _testObject;
        private readonly IHttpContextAccessor _fakeContextAccessor;
        private readonly Mock<IInventoryRepository> _mockInventory;
        private List<Beer> _testData { get; } =
            new List<Beer>
            {
                new Beer { Id = new Random().Next(), Barrelage = new Random().Next(), Name = Guid.NewGuid().ToString(), Style = Guid.NewGuid().ToString() },
                new Beer { Id = new Random().Next(), Barrelage = new Random().Next(), Name = Guid.NewGuid().ToString(), Style = Guid.NewGuid().ToString() },
                new Beer { Id = new Random().Next(), Barrelage = new Random().Next(), Name = Guid.NewGuid().ToString(), Style = Guid.NewGuid().ToString() },
                new Beer { Id = new Random().Next(), Barrelage = new Random().Next(), Name = Guid.NewGuid().ToString(), Style = Guid.NewGuid().ToString() },
                new Beer { Id = new Random().Next(), Barrelage = new Random().Next(), Name = Guid.NewGuid().ToString(), Style = Guid.NewGuid().ToString() }
            };

        public InventoryQuerySpec()
        {
            var clientId = Guid.NewGuid();
            InventoryData.Clients.Add(clientId);

            _fakeContextAccessor = new HttpContextAccessor();
            _fakeContextAccessor.HttpContext = new DefaultHttpContext();
            _fakeContextAccessor.HttpContext.Request.Headers["Authorization"] = $"Bearer {clientId}";

            _mockInventory = new Mock<IInventoryRepository>();
            _mockInventory.Setup(m => m.DoesClientExist(clientId)).ReturnsAsync(true);
            _testObject = new InventoryQuery(_fakeContextAccessor, _mockInventory.Object);
        }

        [Fact]
        public async void Constructor_WhatsOnTap_ReturnsAllItemsInInventory()
        {
            _mockInventory.Setup(m => m.All(It.IsAny<Guid>())).ReturnsAsync(_testData);

            var field = _testObject.GetField("whatsOnTap");
            var actual = await (Task<object>)field.Resolver
                .Resolve(new ResolveFieldContext());

            _mockInventory.Verify(m => m.All(It.IsAny<Guid>()));

            Assert.Equal(_testData.Count, ((List<Beer>)actual).Count);
        }

        [Fact]
        public async void Constructor_WhatsOnTap_ThrowsUnauthorizedAccessException()
        {
            await ThrowsUnauthorizedAccessException(_testObject.GetField("whatsOnTap"), _fakeContextAccessor);
        }

        [Fact]
        public async void Constructor_FindBeer_RetunsInventoryItem()
        {
            var expected = _testData.First();

            _mockInventory.Setup(m => m.Find(It.IsAny<Guid>(), It.IsAny<int>())).ReturnsAsync(expected);

            var field = _testObject.GetField("findBeer");
            var actual = await (Task<object>)field.Resolver
                .Resolve(new ResolveFieldContext { Arguments = new Dictionary<string, object> { { "id", expected.Id } } }) as Beer;

            _mockInventory.Verify(m => m.Find(It.IsAny<Guid>(), It.IsAny<int>()));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void Constructor_FindBeer_ThrowsUnauthorizedAccessException()
        {
            await ThrowsUnauthorizedAccessException(_testObject.GetField("findBeer"), _fakeContextAccessor);
        }
    }
}