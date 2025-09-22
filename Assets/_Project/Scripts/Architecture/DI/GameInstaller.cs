using Architecture._Project.Scripts.Architecture;
using Architecture._Project.Scripts.Architecture.GameResources;
using Reflex.Core;
using UnityEngine;

namespace Architecture._Project.Scripts.Architecture.DI
{
    public class GameInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private GameResourceList _gameResourceList;
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            var inputSystem = new InputSystem_Actions();
            var cameraInputSystem = new CameraInputSystem(inputSystem);
            containerBuilder.AddSingleton(cameraInputSystem, typeof(ICameraInputService));
            
            var globalResourceStorage = new GlobalResourceStorage(_gameResourceList);
            containerBuilder.AddSingleton(globalResourceStorage, typeof(IGlobalResourceStorage));
        }
    }
}