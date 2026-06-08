using UnityEngine;

namespace SubjectA04.HealthSystem
{
    public class Health : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField, Min(1f)] private float maxHealth = 100f;

        private float currentHealth;
        private bool isDead;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        private void Awake()
        {
            // Every object starts alive with full health.
            currentHealth = maxHealth;
            isDead = false;
        }

        public void TakeDamage(float damage)
        {
            if (isDead || damage <= 0f)
            {
                return;
            }

            // Clamp at zero so health never becomes negative.
            currentHealth = Mathf.Max(currentHealth - damage, 0f);
            Debug.Log($"{name} received {damage} damage. Current health: {currentHealth}/{maxHealth}", this);

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

            // Clamp at maxHealth so healing cannot overheal the object.
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            Debug.Log($"{name} healed {amount}. Current health: {currentHealth}/{maxHealth}", this);
        }

        private void Die()
        {
            if (isDead)
            {
                return;
            }

            isDead = true;
            Debug.Log($"{name} died.", this);

            // For now death is simple: remove this object from the scene.
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            maxHealth = Mathf.Max(1f, maxHealth);
        }
    }
}
