using UnityEngine;

namespace Architecture.Units.Configs
{
    [CreateAssetMenu(fileName = "HealthConfig", menuName = "Configs/Health")]
    public class HealthConfig : ScriptableObject
    {
        [SerializeField] private int _maxHealth = 100;
      
        public int MaxHealth => _maxHealth;
    }
}