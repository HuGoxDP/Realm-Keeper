using System;

namespace Architecture.Units
{
    [Serializable]
    public struct MovementSettings
    {
        public float moveSpeed;
        public float rotationSpeed;
        public float stoppingDistance;
        public bool autoBraking;

        public static MovementSettings Default => new MovementSettings
        {
            moveSpeed = 3.5f,
            rotationSpeed = 120f,
            stoppingDistance = 0.5f,
            autoBraking = true
        };
    }
}