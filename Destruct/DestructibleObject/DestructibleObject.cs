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
            Debug.Log($"{gameObject.name} получил {amount} урона. Текущее здоровье: {currentHealth}/{maxHealth}");
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
            Debug.Log($"Создаём {group.fragmentCount} фрагментов при здоровье <= {currentHealth}");
            for (int i = 0; i < group.fragmentCount; i++)
            {
                GameObject fragment = Instantiate(group.fragmentPrefab, transform.position, Quaternion.identity);
                fragment.transform.parent = null;
                fragment.transform.localScale = Vector3.one * 0.5f;

                Rigidbody rb = fragment.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(500f, hitPoint, 5f);
                }
                else
                {
                    Debug.LogWarning($"Фрагмент {fragment.name} не имеет компонента Rigidbody.");
                }
            }

            Collider mainCollider = GetComponent<Collider>();
            if (mainCollider != null)
                mainCollider.enabled = false;

            Renderer mainRenderer = GetComponent<Renderer>();
            if (mainRenderer != null)
                mainRenderer.enabled = false;

            Debug.Log($"{gameObject.name} разрушился на фрагменты!");
        }

        void Die()
        {
            Debug.Log($"{gameObject.name} полностью уничтожен!");
            Destroy(gameObject);
        }
    }
}
