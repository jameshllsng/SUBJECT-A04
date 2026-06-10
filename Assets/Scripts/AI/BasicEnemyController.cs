using SubjectA04.HealthSystem;
using UnityEngine;

namespace SubjectA04.AI
{
    public class BasicEnemyController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField, Min(0.1f)] private float detectionRange = 25f;
        [SerializeField] private bool startsChasing;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float moveSpeed = 2.5f;
        [SerializeField, Min(0f)] private float turnSpeed = 8f;

        [Header("Attack")]
        [SerializeField, Min(0.1f)] private float attackRange = 1.5f;
        [SerializeField, Min(0f)] private float attackDamage = 10f;
        [SerializeField, Min(0f)] private float attackCooldown = 1f;

        private float nextAttackTime;
        private Health health;
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        public bool IsChasing { get; private set; }

        private void Awake()
        {
            health = GetComponent<Health>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            IsChasing = startsChasing;
            AssignTargetIfNeeded();

            if (health != null)
            {
                health.Died += HandleDeath;
            }
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died -= HandleDeath;
            }
        }

        private void Update()
        {
            if (health == null || health.IsDead)
            {
                return;
            }

            if (target == null)
            {
                AssignTargetIfNeeded();
                return;
            }

            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.y = 0f;

            float distanceToTarget = directionToTarget.magnitude;
            if (distanceToTarget <= attackRange)
            {
                if (directionToTarget.sqrMagnitude > Mathf.Epsilon)
                {
                    FaceTarget(directionToTarget);
                }

                TryAttack();
                return;
            }

            if (!IsChasing || distanceToTarget > detectionRange)
            {
                return;
            }

            if (directionToTarget.sqrMagnitude > Mathf.Epsilon)
            {
                FaceTarget(directionToTarget);
                MoveTowardsTarget(directionToTarget);
            }
        }

        public void ActivateChase()
        {
            IsChasing = true;
            Debug.Log($"{name} chase activated.", this);
        }

        public void ResetEnemy()
        {
            gameObject.SetActive(true);
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            health.ResetHealth();
            IsChasing = startsChasing;
            nextAttackTime = 0f;
            Debug.Log($"{name} reset.", this);
        }

        private void MoveTowardsTarget(Vector3 directionToTarget)
        {
            transform.position += directionToTarget.normalized * moveSpeed * Time.deltaTime;
        }

        private void FaceTarget(Vector3 directionToTarget)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime);
        }

        private void TryAttack()
        {
            if (Time.time < nextAttackTime)
            {
                return;
            }

            IDamageable damageable = target.GetComponentInParent<IDamageable>();
            if (damageable == null)
            {
                return;
            }

            nextAttackTime = Time.time + attackCooldown;
            damageable.TakeDamage(attackDamage);
            Debug.Log($"{name} attacked {target.name} for {attackDamage} damage.", this);
        }

        private void AssignTargetIfNeeded()
        {
            if (target != null)
            {
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            detectionRange = Mathf.Max(0.1f, detectionRange);
            moveSpeed = Mathf.Max(0f, moveSpeed);
            turnSpeed = Mathf.Max(0f, turnSpeed);
            attackRange = Mathf.Clamp(attackRange, 0.1f, detectionRange);
            attackDamage = Mathf.Max(0f, attackDamage);
            attackCooldown = Mathf.Max(0f, attackCooldown);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
