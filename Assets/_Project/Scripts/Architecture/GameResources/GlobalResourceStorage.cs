using System.Collections.Generic;
using System;
using R3;
using UnityEngine;
using ZLinq;

namespace Architecture._Project.Scripts.Architecture.GameResources
{
    public interface IGlobalResourceStorage
    { 
        void AddResource(IGameResource resource, int amount);
        bool TryRemoveResource(IGameResource resource, int amount);
        IEnumerable<KeyValuePair<IGameResource, ReactiveProperty<int>>> GetAllResources();
        int GetResourceAmount(IGameResource resource);
    }
    
    public class GlobalResourceStorage : IGlobalResourceStorage
    {
        private readonly IGameResourceList _resourceList;
        private readonly Dictionary<IGameResource, ReactiveProperty<int>> _resources;
        
        public GlobalResourceStorage(IGameResourceList resourceList)
        {
            _resourceList = resourceList;
            _resources = new Dictionary<IGameResource, ReactiveProperty<int>>(_resourceList.Resources.AsValueEnumerable().Count());
            
            // Generate Resources
            InitializeResource();
        }

        private void InitializeResource()
        {
            foreach (var resource in _resourceList.Resources)
            {
                _resources.Add(resource, new ReactiveProperty<int>(0));
            }
        }

        public void AddResource(IGameResource resource, int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException($"Amount of {resource.Name} to remove must be greater than zero");
            if (resource == null) throw new ArgumentNullException($"Resource cannot be null");
            
            if (!_resources.TryGetValue(resource, out var gameResource))
            {
                throw new ArgumentOutOfRangeException($"{resource.Name} not found");
            }
            
            gameResource.Value += Mathf.Clamp(amount, 0, 999999);
        }

        public bool TryRemoveResource(IGameResource resource, int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException($"Amount of {resource.Name} to remove must be greater than zero");
            if (resource == null) throw new ArgumentNullException($"Resource cannot be null");
            
            if (!_resources.TryGetValue(resource, out var gameResource))
            {
                throw new ArgumentOutOfRangeException($"{resource.Name} not found");
            }
            
            if (gameResource.Value < amount) return false;
            
            gameResource.Value -= Mathf.Clamp(amount, 0, 999999);
            return true;
        }

        public int GetResourceAmount(IGameResource resource)
        {
            if (resource == null) throw new ArgumentNullException($"Resource cannot be null");

            if (_resources.TryGetValue(resource, out var gameResource))
            {
                return gameResource.Value;
            }
            
            throw new ArgumentOutOfRangeException($"{resource.Name} not found");
        }
        
        public IEnumerable<KeyValuePair<IGameResource, ReactiveProperty<int>>> GetAllResources()
        {
            return _resources;
        }
        
    }
}