using Architecture.GameResources;
using Architecture.InputSystems;
using Reflex.Core;
using UnityEngine;

namespace Architecture.DI
{
    public class GameInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private GameResourceList _gameResourceList;
        [SerializeField] private UnityEngine.Camera _camera;
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            if (_gameResourceList == null) 
                throw new System.ArgumentNullException(nameof(_gameResourceList), "GameResourceList cannot be null");
            if (_camera == null)
                throw new System.ArgumentNullException(nameof(_camera), "Camera cannot be null");
            
            containerBuilder.AddSingleton(_camera, typeof(UnityEngine.Camera));
            
            var inputSystem = new InputSystem_Actions();
            var cameraInputSystem = new CameraInputSystem(inputSystem);
            var playerInput = new PlayerInputService(inputSystem);
            
            containerBuilder.AddSingleton(cameraInputSystem, typeof(ICameraInputService));
            containerBuilder.AddSingleton(playerInput, typeof(IPlayerInputService));
            
            var globalResourceStorage = new GlobalResourceStorage(_gameResourceList);
            containerBuilder.AddSingleton(globalResourceStorage, typeof(IGlobalResourceStorage));
            
            var unitManager = new UnitManager();
            containerBuilder.AddSingleton(unitManager, typeof(IUnitManager));
            
            var unitSelectionSystem = FindFirstObjectByType<UnitSelectionSystem>();
            containerBuilder.AddSingleton(unitSelectionSystem, typeof(IUnitSelectionSystem));
            
            Debug.Log("GameInstaller Installed");
        }
    }

}