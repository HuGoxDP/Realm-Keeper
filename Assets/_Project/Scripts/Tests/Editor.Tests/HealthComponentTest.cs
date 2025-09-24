using System.Collections;
using Architecture.Units;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class HealthComponentTest
    {
        private IHealthComponent _healthComponent;

        [SetUp]
        public void Setup()
        {
            _healthComponent = new HealthComponent(15);
            _healthComponent.Enable();
        }

        //TakeDamage
        [Test]
        public void TakeDamage_ValidDamage_ReducesHealth()
        {
            _healthComponent.TakeDamage(5);
            Assert.AreEqual(10, _healthComponent.Health.Value);
        }

        [Test]
        public void TakeDamage_ExcessiveDamage_SetsHealthToZeroAndIsDead()
        {
            _healthComponent.TakeDamage(20);
            Assert.AreEqual(0, _healthComponent.Health.Value);
            Assert.IsTrue(_healthComponent.IsDead.Value);
        }

        [Test]
        public void TakeDamage_WhenDisabled_DoesNotChangeHealth()
        {
            _healthComponent.Disable();
            _healthComponent.TakeDamage(5);
            Assert.AreEqual(15, _healthComponent.Health.Value);
        }

        [Test]
        public void TakeDamage_NegativeDamage_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(()
                => _healthComponent.TakeDamage(-5)
            );
        }

        [Test]
        public void TakeDamage_ZeroDamage_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(()
                => _healthComponent.TakeDamage(0)
            );
        }

        [Test]
        public void TakeDamage_WhenDead_DoesNotChangeHealth()
        {
            _healthComponent.TakeDamage(15);
            _healthComponent.TakeDamage(5);
            Assert.AreEqual(0, _healthComponent.Health.Value);
        }

        //Heal
        [Test]
        public void Heal_ValidAmount_IncreasesHealth()
        {
            _healthComponent.TakeDamage(5);
            _healthComponent.Heal(3);
            Assert.AreEqual(13, _healthComponent.Health.Value);
        }

        [Test]
        public void Heal_ExcessiveAmount_SetsHealthToMax()
        {
            _healthComponent.TakeDamage(10);
            _healthComponent.Heal(20);
            Assert.AreEqual(15, _healthComponent.Health.Value);
        }

        [Test]
        public void Heal_WhenDisabled_DoesNotChangeHealth()
        {
            _healthComponent.TakeDamage(5);

            _healthComponent.Disable();
            _healthComponent.Heal(5);
            Assert.AreEqual(10, _healthComponent.Health.Value);
        }

        [Test]
        public void Heal_NegativeAmount_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(()
                => _healthComponent.Heal(-5)
            );
        }

        [Test]
        public void Heal_ZeroAmount_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(()
                => _healthComponent.Heal(0)
            );
        }

        [Test]
        public void Heal_WhenDead_DoesNotChangeHealth()
        {
            _healthComponent.TakeDamage(15);
            _healthComponent.Heal(5);
            Assert.AreEqual(0, _healthComponent.Health.Value);
        }
        
        //ResetHealth
        [Test]
        public void ResetHealth_SetsHealthToMax()
        {
            _healthComponent.TakeDamage(10);
            _healthComponent.ResetHealth();
            Assert.AreEqual(15, _healthComponent.Health.Value);
        }

        [Test]
        public void ResetHealth_WhenDisabled_DoesNotChangeHealth()
        {
            _healthComponent.TakeDamage(10);
            _healthComponent.Disable();
            _healthComponent.ResetHealth();
            Assert.AreEqual(5, _healthComponent.Health.Value);
        }

        [Test]
        public void ResetHealth_WhenDead_DoesNotChangeHealth()
        {
            _healthComponent.TakeDamage(15);
            _healthComponent.ResetHealth();
            Assert.AreEqual(0, _healthComponent.Health.Value);
        }
    }
}