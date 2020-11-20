using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoesTavern.Api.Model;
using MoesTavern.Api.Schema;
using MoesTavern.Data.Data;
using MoesTavern.Data.Model;
using MoesTavern.Data.Repository;
using GraphQL;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace MoesTavern.Api.Tests
{
    public class InventoryMutationSpec : BaseSpec
    {
        private readonly InventoryMutation _testObject;
        private readonly IHttpContextAccessor _fakeContextAccessor;
        private readonly Mock<IInventoryRepository> _mockInventory;

        public InventoryMutationSpec()
        {
            var clientId = Guid.NewGuid();
            InventoryData.Clients.Add(clientId);

            _fakeContextAccessor = new HttpContextAccessor();
            _fakeContextAccessor.HttpContext = new DefaultHttpContext();
            _fakeContextAccessor.HttpContext.Request.Headers["Authorization"] = $"Bearer {clientId}";

            _mockInventory = new Mock<IInventoryRepository>();
            _mockInventory.Setup(m => m.DoesClientExist(clientId)).ReturnsAsync(true);
            _testObject = new InventoryMutation(_fakeContextAccessor, _mockInventory.Object);
        }

        [Fact]
        public async void Constructor_AddBeer_WithCorrectIds()
        {
            var expected = new Beer
                        { 
                            Id = new Random().Next(),
                            Barrelage = new Random().Next(),
                            Name = Guid.NewGuid().ToString(),
                            Style = Guid.NewGuid().ToString()
                        };

            _mockInventory.Setup(m => m.Remove(It.IsAny<Guid>(), expected.Id));

            var field = _testObject.GetField("addBeer");
            var actual = await (Task<object>)field.Resolver
                .Resolve(new ResolveFieldContext{ 
                        Arguments = new Dictionary<string, object> {
                            {"beer", new Dictionary<string, object> 
                                {
                                    { "id", expected.Id },
                                    { "barrelage", Convert.ToDecimal(expected.Barrelage) },
                                    { "name", expected.Name },
                                    { "style", expected.Style },
                                }
                    }}}) as Beer;

            _mockInventory.Verify(m => m.Add(It.IsAny<Guid>(), It.IsAny<Beer>()));
            
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Barrelage, actual.Barrelage);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Style, actual.Style);
        }

        [Fact]
        public async void Constructor_AddBeer_ThrowsUnauthorizedAccessException()
        {
            await ThrowsUnauthorizedAccessException(_testObject.GetField("addBeer"), _fakeContextAccessor);
        }

        [Fact]
        public async void Constructor_DeleteBeer_WithCorrectIds()
        {
            var expectedInventoryId = new Random().Next();

            _mockInventory.Setup(m => m.Remove(It.IsAny<Guid>(), expectedInventoryId));

            var field = _testObject.GetField("deleteBeer");
            var actualInventoryId = await (Task<object>)field.Resolver
                .Resolve(new ResolveFieldContext { Arguments = new Dictionary<string, object> {{"id", expectedInventoryId }}});

            _mockInventory.Verify(m => m.Remove(It.IsAny<Guid>(), expectedInventoryId));

            Assert.Equal(expectedInventoryId, (int)actualInventoryId);
        }

        [Fact]
        public async void Constructor_DeleteBeer_ThrowsUnauthorizedAccessException()
        {
            await ThrowsUnauthorizedAccessException(_testObject.GetField("deleteBeer"), _fakeContextAccessor);
        }
        
        [Theory]
        [InlineData(ContainerType.HalfPint, 1, 1, .998)]
        [InlineData(ContainerType.Pint, 1, 1, .996)]
        [InlineData(ContainerType.Growler, 1, 10, 9.984)]
        [InlineData(ContainerType.SixthBarrel, 6, 1, 0.001)]
        [InlineData(ContainerType.QuarterBarrel, 3, 1, .25)]
        [InlineData(ContainerType.HalfBarrel, 2, 1, 0)]
        public async void Constructor_CallsSoldBeer_CalcuatingCorrectBarrelage(ContainerType container, int quantitySold, double startingBarrelage, double expectedBarrelage)
        {
            var expectedInventoryId = new Random().Next();

            var soldBeer = new SoldBeer{ Id = expectedInventoryId, Quantity = quantitySold, Container = container };
            var expected = new Beer{ Id = expectedInventoryId, Barrelage = startingBarrelage, Name = Guid.NewGuid().ToString(), Style = Guid.NewGuid().ToString()};

            _mockInventory.Setup(m => m.Find(It.IsAny<Guid>(), expectedInventoryId)).ReturnsAsync(expected);

            var field = _testObject.GetField("soldBeer");
            var actual = await (Task<object>)field.Resolver
                .Resolve(new ResolveFieldContext{ 
                                Arguments = new Dictionary<string, object> {
                                    {"beer", new Dictionary<string, object> 
                                        {
                                            { "id", soldBeer.Id },
                                            { "quantity", soldBeer.Quantity },
                                            { "container", soldBeer.Container }
                                        }
                                }}}) as Beer;

            _mockInventory.Verify(m => m.Find(It.IsAny<Guid>(), expectedInventoryId));
            _mockInventory.Verify(m => m.Update(It.IsAny<Guid>(), expectedInventoryId, expectedBarrelage));

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Barrelage, actual.Barrelage);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Style, actual.Style);
        }

        [Fact]
        public async void Constructor_SoldBeer_ThrowsUnauthorizedAccessException()
        {
            await ThrowsUnauthorizedAccessException(_testObject.GetField("soldBeer"), _fakeContextAccessor);
        }
    }
}