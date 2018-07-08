using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AzureCosmosDB.Dao;
using AzureCosmosDB.Models;
using AzureCosmosDB.Repositories;

namespace AzureCosmosDB.Tests
{
    /// <summary>
    ///     Tests for the TestItem Repository
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    [ExcludeFromCodeCoverage]
    public class TestItemRepositoryTests
    {
        /// <summary>
        ///     The context for testing
        /// </summary>
        DocumentDBContext TestingContext;
        
        /// <summary>
        ///     The repository being tested
        /// </summary>
        private TestItemRepository _repository;

        /// <summary>
        ///     UserId for Testing
        /// </summary>
        private string _testUserId;

        [OneTimeSetUp]
        public void Setup()
        {
            TestingContext = new DocumentDBContext()
            {
                DatabaseId = "TestDb-" + Guid.NewGuid().ToString(),
                CollectionId = "TestItemRepositoryTests-" + Guid.NewGuid().ToString()
            };

            _testUserId = Guid.NewGuid().ToString();

            _repository = new TestItemRepository(TestingContext);
            _repository.CreateDatabaseAsync(TestingContext.DatabaseId).Wait();
            _repository.CreateCollectionAsync(TestingContext.DatabaseId, TestingContext.CollectionId).Wait();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _repository.DeleteCollectionAsync(TestingContext.DatabaseId, TestingContext.CollectionId).Wait();
            _repository.DeleteDatabaseAsync(TestingContext.DatabaseId).Wait();
        }

        [Test]
        public async Task CreateItemAsync()
        {
            TestItem item = new TestItem()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _testUserId,
                ItemType = typeof(TestItem).ToString(),
                Content = "Item For Testing"
            };
            await _repository.SaveItemAsync(item);

            TestItem readItem = await _repository.GetItemAsync(item.Id);
            Assert.AreEqual(item, readItem);
        }

        [Test]
        public async Task UpdateItemAsync()
        {
            TestItem item = new TestItem()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _testUserId,
                ItemType = typeof(TestItem).ToString(),
                Content = "Item For Testing"
            };
            await _repository.SaveItemAsync(item);

            TestItem readItem = await _repository.GetItemAsync(item.Id);
            Assert.AreEqual(item, readItem);

            readItem.Content = "Item is Changed";
            await _repository.SaveItemAsync(readItem);

            TestItem updatedItem = await _repository.GetItemAsync(item.Id);
            Assert.AreNotEqual(item, updatedItem);
            Assert.AreEqual(readItem, updatedItem);
        }

        [Test]
        public async Task RetreiveAllAsync()
        {
            IList<TestItem> items = new List<TestItem>();
            items.Add(new TestItem()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _testUserId,
                ItemType = typeof(TestItem).ToString(),
                Content = "Item A For Testing"
            });
            items.Add(new TestItem()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _testUserId,
                ItemType = typeof(TestItem).ToString(),
                Content = "Item B For Testing"
            });
            items.Add(new TestItem()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _testUserId,
                ItemType = typeof(TestItem).ToString(),
                Content = "Item C For Testing"
            });

            foreach (TestItem item in items)
            {
                await _repository.SaveItemAsync(item);
            }

            IEnumerable<TestItem> readItems = await _repository.GetItemsAsync(x => x.UserId == _testUserId);
            CollectionAssert.Contains(readItems, items[0]);
            CollectionAssert.Contains(readItems, items[1]);
            CollectionAssert.Contains(readItems, items[2]);
        }

        [Test]
        public async Task DeleteItemAsync()
        {
            IList<TestItem> items = new List<TestItem>();
            items.Add(new TestItem()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _testUserId,
                ItemType = typeof(TestItem).ToString(),
                Content = "Item A For Testing"
            });
            items.Add(new TestItem()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _testUserId,
                ItemType = typeof(TestItem).ToString(),
                Content = "Item B For Testing"
            });
            items.Add(new TestItem()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _testUserId,
                ItemType = typeof(TestItem).ToString(),
                Content = "Item C For Testing"
            });

            foreach (TestItem item in items)
            {
                await _repository.SaveItemAsync(item);
            }

            await _repository.DeleteItemAsync(items[0].Id);

            IEnumerable<TestItem> readItems = await _repository.GetItemsAsync(x => x.UserId == _testUserId);
            CollectionAssert.DoesNotContain(readItems, items[0]);
        }

    }
}