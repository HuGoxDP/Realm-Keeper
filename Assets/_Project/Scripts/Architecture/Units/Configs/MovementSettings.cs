using UnityEngine;

namespace Architecture.Units.Configs
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Configs/Movement")]
    public class MovementConfig : ScriptableObject
    {
       [SerializeField] private float _moveSpeed;
       [SerializeField] private float _rotationSpeed;
       [SerializeField] private float _stoppingDistance;
       [SerializeField] private bool _autoBraking;
        
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
        public float StoppingDistance => _stoppingDistance;
        public bool AutoBraking => _autoBraking;
    }
}