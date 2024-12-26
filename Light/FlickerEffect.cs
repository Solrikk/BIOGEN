using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class FlickerEffect : MonoBehaviour
{
    // Минимальная и максимальная интенсивность света
    public float minIntensity = 0f;
    public float maxIntensity = 8f;

    // Минимальное и максимальное время между изменениями интенсивности
    public float minFlickerTime = 0.05f;
    public float maxFlickerTime = 0.2f;

    // Продолжительность эффекта мерцания (в секундах). Если 0, эффект бесконечный
    public float duration = 0f;

    private Light _light;
    private bool _isFlickering = false;

    void Start()
    {
        _light = GetComponent<Light>();
        if (_light == null)
        {
            Debug.LogError("FlickerEffect: Компонент Light не найден!");
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
            // Случайная интенсивность света
            _light.intensity = Random.Range(minIntensity, maxIntensity);

            // Случайное время до следующего изменения
            float waitTime = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(waitTime);

            if (duration > 0)
            {
                elapsed += waitTime;
                if (elapsed >= duration)
                {
                    _isFlickering = false;
                    // Возврат к исходной интенсивности после завершения эффекта
                    _light.intensity = maxIntensity;
                }
            }
        }
    }
}
