using System;
using R3;
using Reflex.Attributes;
using UnityEngine;

namespace Architecture.GameResources
{
    public class GameResourceUIHandler : MonoBehaviour, IDisposable
    {
        [SerializeField] private GameResourceItemUI _prefab;
        [SerializeField] private RectTransform _container;
        
        private IGlobalResourceStorage _globalResourceStorage;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        [Inject]
        private void Construct(IGlobalResourceStorage globalResourceStorage)
        {
            _globalResourceStorage = globalResourceStorage;
        }
        
        private void Start()
        {
            CreateResourceUIElements();
        }

        private void CreateResourceUIElements()
        {
            var resources = _globalResourceStorage.GetAllResources();
            foreach (var (gameResource, gameResourceReactiveProperty) in resources)
            {
                var gameResourceItemUI = Instantiate(_prefab, _container);
                
               //TODO: placement of UI elements
               
                gameResourceItemUI.Initialize(gameResource, gameResourceReactiveProperty.Value);
                gameResourceReactiveProperty.Subscribe(amount => gameResourceItemUI.UpdateAmount(amount))
                    .AddTo(_disposable);
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}