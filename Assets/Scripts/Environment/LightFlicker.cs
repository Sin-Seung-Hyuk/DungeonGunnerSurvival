using UnityEngine;
using UnityEngine.Rendering.Universal; // keep this namespace in for when 2D lights become non experimental;
using UnityEngine.Experimental.Rendering.Universal;

[DisallowMultipleComponent]
public class LightFlicker : MonoBehaviour
{
    private Light2D light2D;
    [SerializeField] private float lightIntensityMin; // ÃÖ¼Ò ¹à±â
    [SerializeField] private float lightIntensityMax; // ÃÖ´ë ¹à±â
    [SerializeField] private float lightFlickerTimeMin; // ±ôºýÀÌ´Â ½Ã°£
    [SerializeField] private float lightFlickerTimeMax;
    private float lightFlickerTimer;

    private void Awake()
    {
        // Light2D ÄÄÆ÷³ÍÆ® °¡Á®¿À±â
        light2D = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);
    }

    private void Update()
    {
        if (light2D == null) return;

        lightFlickerTimer -= Time.deltaTime;

        if (lightFlickerTimer < 0f)
        {
            // ±ôºýÀÌ´Â ½Ã°£ ·£´ýÀ¸·Î °áÁ¤
            lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);

            RandomiseLightIntensity();
        }
    }

    private void RandomiseLightIntensity()
    {
        light2D.intensity = Random.Range(lightIntensityMin, lightIntensityMax);
    }
}
