using UnityEngine;

namespace Unity.FPS.Game
{
    [System.Serializable]
    public struct FragmentGroup
    {
        public float healthThreshold;
        public GameObject fragmentPrefab;
        public int fragmentCount;
    }

    public class DestructibleObject : MonoBehaviour
    {
        public float maxHealth = 100f;
        private float currentHealth;
        public FragmentGroup[] fragmentGroups;
        private int currentFragmentsActivated = 0;

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount, Vector3 hitPoint, Vector3 hitDirection)
        {
            currentHealth -= amount;
            Debug.Log($"{gameObject.name} received {amount} damage. Current health: {currentHealth}/{maxHealth}");
            foreach (var group in fragmentGroups)
            {
                if (currentHealth <= group.healthThreshold && currentFragmentsActivated < group.fragmentCount)
                {
                    ActivateFragments(group, hitPoint, hitDirection);
                    currentFragmentsActivated += group.fragmentCount;
                }
            }
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        void ActivateFragments(FragmentGroup group, Vector3 hitPoint, Vector3 hitDirection)
        {
            Debug.Log($"Creating {group.fragmentCount} fragments at health <= {currentHealth}");
            for (int i = 0; i < group.fragmentCount; i++)
            {
                GameObject fragment = Instantiate(group.fragmentPrefab, transform.position, Quaternion.identity);
                fragment.transform.parent = null;

                Rigidbody rb = fragment.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(500f, hitPoint, 5f);
                }
                else
                {
                    Debug.LogWarning($"Fragment {fragment.name} does not have a Rigidbody component.");
                }
            }

            Collider mainCollider = GetComponent<Collider>();
            if (mainCollider != null)
                mainCollider.enabled = false;

            Renderer mainRenderer = GetComponent<Renderer>();
            if (mainRenderer != null)
                mainRenderer.enabled = false;

            Debug.Log($"{gameObject.name} has been destroyed into fragments!");
        }

        void Die()
        {
            Debug.Log($"{gameObject.name} has been completely destroyed!");
            Destroy(gameObject);
        }
    }
}
