using Architecture.Units.Core;
using UnityEngine;

namespace Architecture.Testing
{
    public class TestDamageZone : MonoBehaviour
    { 
        private void OnTriggerEnter(Collider other)
        {
            var baseUnit = other.gameObject.GetComponent<BaseUnit>();
            if (baseUnit != null)
            {
                baseUnit.ApplyDamage(10);
            }
        }
    }
}