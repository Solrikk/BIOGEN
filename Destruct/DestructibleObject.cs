using UnityEngine;

namespace Unity.FPS.Game
{
    [System.Serializable]
    public struct FragmentGroup
    {
        public float healthThreshold;
        public GameObject[] fragments;
    }

    public class DestructibleObject : MonoBehaviour
    {
        public float maxHealth = 100f;
        private float currentHealth;
        public FragmentGroup[] fragmentGroups;

        void Start()
        {
            currentHealth = maxHealth;
            foreach (var group in fragmentGroups)
            {
                foreach (var fragment in group.fragments)
                {
                    fragment.SetActive(false);
                }
            }
        }

        public void TakeDamage(float amount, Vector3 hitPoint, Vector3 hitDirection)
        {
            currentHealth -= amount;
            Debug.Log($"{gameObject.name} получил {amount} урона. Текущее здоровье: {currentHealth}/{maxHealth}");
            foreach (var group in fragmentGroups)
            {
                if (currentHealth <= group.healthThreshold && !AreFragmentsActive(group.fragments))
                {
                    ActivateFragments(group.fragments, hitPoint, hitDirection);
                }
            }
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        bool AreFragmentsActive(GameObject[] fragments)
        {
            foreach (var fragment in fragments)
            {
                if (fragment.activeSelf)
                    return true;
            }
            return false;
        }

        void ActivateFragments(GameObject[] fragments, Vector3 hitPoint, Vector3 hitDirection)
        {
            Debug.Log($"Активируем {fragments.Length} фрагментов при здоровье <= {currentHealth}");
            foreach (var fragment in fragments)
            {
                fragment.SetActive(true);
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
