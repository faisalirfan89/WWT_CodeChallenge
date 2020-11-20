using System;
using System.Collections.Generic;
using MoesTavern.Data.Data;
using MoesTavern.Data.Model;
using MoesTavern.Data.Repository;
using Xunit;

namespace MoesTavern.Data.Tests
{
    public class InventoryRespositorySpec
    {
        private readonly Dictionary<string, Beer> _data;
        private readonly InventoryRepository _testObject;

        private Guid _clientId { get; } = Guid.NewGuid();

        public InventoryRespositorySpec()
        {
            InventoryData.Clients.Add(_clientId);
            _data = InventoryData.Cache();
            
            _testObject = new InventoryRepository(_data, InventoryData.Clients);
        }

        public string CreateKey(int id) => $"{_clientId}-{id}";

        [Fact]
        public async void All_ReturnsData_WhenGivenCorrectId()
        {
            var actual = await _testObject.All(_clientId);
            Assert.True(actual.Count > 0 );
        }

        [Fact]
        public async void All_ReturnsNoData_WhenGivenIncorrectId()
        {
            var actual = await _testObject.All(Guid.NewGuid());
            Assert.True(actual.Count == 0 );
        }

        [Fact]
        public async void Find_ReturnsCorrectItem_WhenGivenCorrectIds()
        {
            var actual = await _testObject.Find(_clientId,1);
            Assert.Equal(1, actual.Id);
        }

        [Fact]
        public async void Find_ReturnsNull_WhenGivenIncorrectIds()
        {
            var actual = await _testObject.Find(_clientId, 0);
            Assert.Null(actual);

            actual = await _testObject.Find(Guid.NewGuid(), 1);
            Assert.Null(actual);
        }

        [Fact]
        public async void Add_AppendsDataSet_WhenGivenObject()
        {
            var expected = new Beer 
                        { 
                            Id = 100, 
                            Barrelage = 100, 
                            Name = "Duff Foo Boo",
                            Style = "Ghost Porter"
                        };

            await _testObject.Add(_clientId, expected);

            Assert.True(_data.TryGetValue(CreateKey(expected.Id), out var actual), "Item not found");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void Update_SetsValues_WhenCorrectIds()
        {
            await _testObject.Update(_clientId, 1, 100);

            Assert.True(_data.TryGetValue(CreateKey(1), out var actual), "Item not found");
            Assert.Equal(100, actual.Barrelage);
        }

        [Fact]
        public async void Update_DoesNotSetValues_WhenItemDoesNotExist()
        {
            await _testObject.Update(_clientId, 11, 100);
 
            Assert.False(_data.TryGetValue(CreateKey(11), out var actual), "Item found");
        }

        [Fact]
        public async void Remove_RemovesItem_WhenCorrectIds()
        {
            var expected = new Beer {  Id = 1 };

            await _testObject.Remove(_clientId, expected.Id);

            Assert.False(_data.TryGetValue(CreateKey(expected.Id), out var val), "Item found");
        }

        [Theory]
        [InlineData("1d740ff1-978b-4e2d-ada1-3a804a772c0c", "1d740ff1-978b-4e2d-ada1-3a804a772c0c", true)]
        [InlineData("1d740ff1-978b-4e2d-ada1-3a804a772c0c", "e277d952-694d-4979-b383-5bcfd536cf04", false)]
        public async void DoesClientExist_ReturnsTrueOrFalse(string clientId, string operand, bool expected)
        {
            InventoryData.Clients.Add(Guid.Parse(clientId));

            var actual = await _testObject.DoesClientExist(Guid.Parse(operand));

            Assert.Equal(expected, actual);
        }
    }
}
