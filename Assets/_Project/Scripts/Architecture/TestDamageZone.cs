using System;
using Architecture.Units;
using UnityEngine;

namespace Architecture
{
    public class TestDamageZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            var baseUnit = other.gameObject.GetComponent<BaseUnit>();
            if (baseUnit != null)
            {
                baseUnit.TakeDamage(10);
            }
        }
    }
}