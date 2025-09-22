using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Architecture._Project.Scripts.Architecture.GameResources
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