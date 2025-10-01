using System.Collections.Generic;
using UnityEngine;

namespace Architecture.GameResources
{
    public interface IGameResourceList
    {
        IEnumerable<IGameResource> Resources { get; }
    }
    
    [CreateAssetMenu(fileName = "new GameResourceList", menuName = "Game/GameResource/Create GameResource List", order = 0)]
    public class GameResourceList : ScriptableObject, IGameResourceList
    {
        public IEnumerable<IGameResource> Resources => _list;
        
        [SerializeField] private List<GameResource> _list;
    }
}