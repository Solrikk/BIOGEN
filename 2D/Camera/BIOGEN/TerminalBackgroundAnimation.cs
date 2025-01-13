
using UnityEngine;
using UnityEngine.UI;

public class TerminalBackgroundAnimation : MonoBehaviour 
{
    public float scanlineSpeed = 0.5f;
    public float glowIntensity = 0.2f;
    private Material material;
    private RawImage backgroundImage;
    private float offset = 0f;

    void Start()
    {
        backgroundImage = GetComponent<RawImage>();
        if (backgroundImage != null)
        {
            material = new Material(Shader.Find("UI/Default"));
            backgroundImage.material = material;
        }
    }

    void Update()
    {
        offset += Time.deltaTime * scanlineSpeed;
        if (offset > 1f) offset -= 1f;
        
        float glow = Mathf.Sin(Time.time) * glowIntensity + 1f;
        if (backgroundImage != null)
        {
            Color currentColor = backgroundImage.color;
            backgroundImage.color = new Color(currentColor.r * glow, 
                                           currentColor.g * glow, 
                                           currentColor.b * glow, 
                                           currentColor.a);
        }
    }
}
