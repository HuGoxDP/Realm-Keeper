using Architecture._Project.Scripts.Architecture;
using Reflex.Core;
using UnityEngine;

namespace Architecture._Project.Scripts.Architecture.DI
{
    public class GameInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            var inputSystem = new InputSystem_Actions();
            var cameraInputSystem = new CameraInputSystem(inputSystem);
            containerBuilder.AddSingleton(cameraInputSystem, typeof(ICameraInputService));
        }
    }
}