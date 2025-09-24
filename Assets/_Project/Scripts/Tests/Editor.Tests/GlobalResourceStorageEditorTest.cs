using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.GameResources;
using NSubstitute;
using ZLinq;

// ReSharper disable once CheckNamespace
namespace GameResources
{
    public class GlobalResourceStorageEditorTest
    {
        private IGameResource _testWoodResource;
        private IGameResourceList _testResourceList;
        private IGlobalResourceStorage _storage;
        
        [SetUp]
        public void Setup()
        {
            _testWoodResource = Substitute.For<IGameResource>();
            _testWoodResource.Name.Returns("Wood");
            
            _testResourceList = Substitute.For<IGameResourceList>();
            _testResourceList.Resources.Returns(new List<IGameResource> {_testWoodResource}); 
            
            _storage = new GlobalResourceStorage(_testResourceList);
        }
        
        [Test]
        public void AddResource_PositiveAmount_AddsToResourceValue()
        {
            _storage.AddResource(_testWoodResource, 10);
            
            Assert.AreEqual(10, _storage.GetResourceAmount(_testWoodResource));
        }
        
        [Test]
        public void AddResource_NegativeAmount_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _storage.AddResource(_testWoodResource, -10));
        }
        
        [Test]
        public void AddResource_ResourceIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _storage.AddResource(null, 10));   
        }

        [Test]
        public void AddResource_ResourceNotInList_ThrowsException()
        {
            var nonExistentResource = Substitute.For<IGameResource>();
            nonExistentResource.Name.Returns("NonExistentResource");            
            
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _storage.AddResource(nonExistentResource , 10));
        }
        
        [Test]
        public void AddResource_MaxAmount_AddsMaxAmountToResourceValue()
        {
            _storage.AddResource(_testWoodResource, 999999);
            
            Assert.AreEqual(999999, _storage.GetResourceAmount(_testWoodResource));
        }

        [Test]
        public void AddResource_ZeroAmount_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _storage.AddResource(_testWoodResource, 0));
        }
        
        [Test]
        public void TryRemoveResource_PositiveAmount_RemovesFromResourceValue()
        {
            _storage.AddResource(_testWoodResource, 100);
            
            _storage.TryRemoveResource(_testWoodResource, 10);
            
            Assert.AreEqual(90, _storage.GetResourceAmount(_testWoodResource));
        }
        
        [Test]
        public void TryRemoveResource_NegativeAmount_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _storage.TryRemoveResource(_testWoodResource, -10));
        }
        
        [Test]
        public void TryRemoveResource_AmountMoreThanCurrent_ReturnsFalseAndDoesNotChangeValue()
        {
            _storage.AddResource(_testWoodResource, 5);
            
            var result = _storage.TryRemoveResource(_testWoodResource, 10);
            
            Assert.AreEqual(false, result);
            Assert.AreEqual(5, _storage.GetResourceAmount(_testWoodResource));
        }
        
        [Test]
        public void TryRemoveResource_ZeroAmount_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _storage.TryRemoveResource(_testWoodResource, 0));
        }
        
        [Test]
        public void TryRemoveResource_ResourceIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => 
                _storage.TryRemoveResource(null, 10));   
        }
        
        [Test]
        public void GetAmount_ResourceNotInList_ThrowsException()
        {
            var nonExistentResource = Substitute.For<IGameResource>();
            nonExistentResource.Name.Returns("NonExistentResource"); 
            
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _storage.GetResourceAmount(nonExistentResource));
        }
        
        [Test]
        public void GetAmount_ResourceIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _storage.GetResourceAmount(null));   
        }
        
        [Test]
        public void GetAmount_ResourceInList_ReturnsAmount()
        {
            _storage.AddResource(_testWoodResource, 10);
            
            Assert.AreEqual(10, _storage.GetResourceAmount(_testWoodResource));
        }
        
       [Test]
        public void GetAllResources_AfterInitialization_ReturnsAllResourcesFromList()
        {
            var resources = _storage.GetAllResources();
            
            Assert.AreEqual(1, resources.AsValueEnumerable().Count());
            Assert.AreEqual(0, _storage.GetResourceAmount(_testWoodResource));
        }

    }
}
