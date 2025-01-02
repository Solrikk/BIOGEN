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

                Renderer renderer = fragment.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.SetFloat("_Mode", 3);
                    renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    renderer.material.SetInt("_ZWrite", 0);
                    renderer.material.DisableKeyword("_ALPHATEST_ON");
                    renderer.material.EnableKeyword("_ALPHABLEND_ON");
                    renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    renderer.material.renderQueue = 3000;
                    Color color = renderer.material.color;
                    color.a = 0.5f;
                    renderer.material.color = color;
                }

                Collider collider = fragment.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.isTrigger = true;
                }

                Rigidbody rb = fragment.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(500f, hitPoint, 5f);
                }
                else
                {
                    Debug.LogWarning($"Fragment {fragment.name} does not have a Rigidbody component.");
                }

                Destroy(fragment, 5f);
            }

            Collider mainCollider = GetComponent<Collider>();
            if (mainCollider != null)
                mainCollider.enabled = false;

            Renderer mainRenderer = GetComponent<Renderer>();
            if (mainRenderer != null)
                mainRenderer.enabled = false;

            Rigidbody mainRigidbody = GetComponent<Rigidbody>();
            if (mainRigidbody != null)
            {
                mainRigidbody.isKinematic = true;
                mainRigidbody.detectCollisions = false;
            }

            Debug.Log($"{gameObject.name} has been destroyed into fragments!");
        }

        void Die()
        {
            Debug.Log($"{gameObject.name} has been completely destroyed!");
            Destroy(gameObject);
        }
    }
}
