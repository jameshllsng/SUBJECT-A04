using SubjectA04.HealthSystem;
using UnityEngine;

namespace SubjectA04.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera playerCamera;

        [Header("Weapon Settings")]
        [SerializeField, Min(0f)] private float damage = 25f;
        [SerializeField, Min(0.1f)] private float range = 100f;
        [SerializeField, Min(0f)] private float cooldown = 0.25f;

        private float nextFireTime;

        private void Awake()
        {
            AssignCameraIfNeeded();
        }

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(0))
            {
                TryShoot();
            }
#else
            Debug.LogError(
                "Weapon uses the old Unity Input Manager. Set Project Settings > Player > Active Input Handling to 'Input Manager (Old)' or 'Both'.",
                this);
            enabled = false;
#endif
        }

        private void TryShoot()
        {
            if (Time.time < nextFireTime)
            {
                return;
            }

            nextFireTime = Time.time + cooldown;
            Shoot();
        }

        private void Shoot()
        {
            if (playerCamera == null)
            {
                Debug.LogWarning("Weapon needs a player camera before it can shoot.", this);
                return;
            }

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            // Draw the shot path in Scene View to make raycast testing easier.
            Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);

            if (!Physics.Raycast(ray, out RaycastHit hit, range))
            {
                return;
            }

            Debug.Log($"{name} hit {hit.collider.name}.", hit.collider);

            Health health = hit.collider.GetComponentInParent<Health>();
            if (health == null)
            {
                return;
            }

            health.TakeDamage(damage);
            Debug.Log($"{name} caused {damage} damage to {health.name}.", health);
        }

        private void AssignCameraIfNeeded()
        {
            if (playerCamera != null)
            {
                return;
            }

            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                return;
            }

            playerCamera = Camera.main;
        }

        private void OnValidate()
        {
            damage = Mathf.Max(0f, damage);
            range = Mathf.Max(0.1f, range);
            cooldown = Mathf.Max(0f, cooldown);
        }
    }
}
