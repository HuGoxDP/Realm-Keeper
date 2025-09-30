using UnityEngine;

namespace Architecture.Units.Data
{
    public class MovementData
    {
        public Vector3? TargetPosition { get; private set; }
        public Transform TargetTransform { get; private set; }
        public bool HasMoveCommand => TargetPosition.HasValue || TargetTransform;
     
        public void SetTarget(Vector3 position)
        {
            TargetPosition = position;
            TargetTransform = null;
        }

        public void SetTarget(Transform transform)
        {
            TargetTransform = transform;
            TargetPosition = null;
        }

        public void Clear()
        {
            TargetPosition = null;
            TargetTransform = null;
        }
        
        public Vector3? GetCurrentTargetPosition()
        {
            if (TargetTransform != null)
                return TargetTransform.position;
            
            return TargetPosition;
        }
    }
}