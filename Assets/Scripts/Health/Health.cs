using System;
using UnityEngine;

namespace SubjectA04.HealthSystem
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField, Min(1f)] private float maxHealth = 100f;
        [SerializeField] private bool destroyOnDeath = true;

        private float currentHealth;
        private bool isDead;
        private MonoBehaviour[] attachedBehaviours;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => isDead;

        public event Action Died;

        private void Awake()
        {
            // Every object starts alive with full health.
            currentHealth = maxHealth;
            isDead = false;
            attachedBehaviours = GetComponents<MonoBehaviour>();
        }

        public void TakeDamage(float damage)
        {
            if (isDead || damage <= 0f)
            {
                return;
            }

            float modifiedDamage = ApplyDamageModifiers(damage);
            if (modifiedDamage <= 0f)
            {
                return;
            }

            currentHealth = Mathf.Max(currentHealth - modifiedDamage, 0f);
            Debug.Log($"{name} received {modifiedDamage} damage. Current health: {currentHealth}/{maxHealth}", this);

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            if (isDead || amount <= 0f)
            {
                return;
            }

            float previousHealth = currentHealth;
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            float restoredHealth = currentHealth - previousHealth;

            if (restoredHealth > 0f)
            {
                Debug.Log($"{name} healed {restoredHealth}. Current health: {currentHealth}/{maxHealth}", this);
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            isDead = false;
            Debug.Log($"{name} health reset to {currentHealth}/{maxHealth}.", this);
        }

        private void Die()
        {
            if (isDead)
            {
                return;
            }

            isDead = true;
            Debug.Log($"{name} died.", this);
            Died?.Invoke();

            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }

        private float ApplyDamageModifiers(float damage)
        {
            float modifiedDamage = damage;

            foreach (MonoBehaviour behaviour in attachedBehaviours)
            {
                if (behaviour is IDamageModifier modifier)
                {
                    modifiedDamage = modifier.ModifyDamage(modifiedDamage);
                }
            }

            return Mathf.Max(0f, modifiedDamage);
        }

        private void OnValidate()
        {
            maxHealth = Mathf.Max(1f, maxHealth);
        }
    }
}
