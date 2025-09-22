using UnityEngine;

namespace Architecture._Project.Scripts.Architecture.GameResources
{
    public interface IGameResource
    {
        string Name { get; }
        Sprite Icon { get; }
    }
    
    [CreateAssetMenu(fileName = "New Resource", menuName = "Game/GameResource/Create GameResource", order = 0)]
    public class GameResource : ScriptableObject, IGameResource
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}