using SubjectA04.AI;
using UnityEngine;

namespace SubjectA04.World
{
    public class PrototypeWorldController : MonoBehaviour
    {
        [SerializeField] private BasicEnemyController enemy;
        [SerializeField] private GameObject[] groundItems;

        public void ActivateEnemy()
        {
            if (enemy != null)
            {
                enemy.ActivateChase();
            }
        }

        public void ResetEnemy()
        {
            if (enemy != null)
            {
                enemy.ResetEnemy();
            }
        }

        public void ResetGroundItems()
        {
            foreach (GameObject groundItem in groundItems)
            {
                if (groundItem != null)
                {
                    groundItem.SetActive(true);
                }
            }

            Debug.Log("Ground items reset.", this);
        }
    }
}
