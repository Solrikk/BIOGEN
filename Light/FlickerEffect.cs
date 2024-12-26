using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class FlickerEffect : MonoBehaviour
{
    public float minIntensity = 0f;
    public float maxIntensity = 8f;

    public float minFlickerTime = 0.05f;
    public float maxFlickerTime = 0.2f;

    public float duration = 0f;

    private Light _light;
    private bool _isFlickering = false;

    void Start()
    {
        _light = GetComponent<Light>();
        if (_light == null)
        {
            Debug.LogError("FlickerEffect: Êîìïîíåíò Light íå íàéäåí!");
            return;
        }

        if (duration > 0)
        {
            StartCoroutine(FlickerCoroutine(duration));
        }
        else
        {
            StartCoroutine(FlickerCoroutine());
        }
    }

    IEnumerator FlickerCoroutine(float duration = 0f)
    {
        _isFlickering = true;
        float elapsed = 0f;

        while (_isFlickering)
        {
            _light.intensity = Random.Range(minIntensity, maxIntensity);

            float waitTime = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(waitTime);

            if (duration > 0)
            {
                elapsed += waitTime;
                if (elapsed >= duration)
                {
                    _isFlickering = false;
                    _light.intensity = maxIntensity;
                }
            }
        }
    }
}
