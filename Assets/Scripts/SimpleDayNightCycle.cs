using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SimpleDayNightCycle : MonoBehaviour
{
    [Range(0f, 24f)]
    public float timeOfDay = 12f; // 0-24 hours
    public float dayLength = 60f; // Seconds for a full cycle

    [Header("Sun Settings")]
    public Light sunLight;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon Settings")]
    public Light moonLight;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Rotation Settings")]
    public Transform sunPivot;
    public float sunRotationOffset = -90f;

    private void Reset()
    {
        // --- Sunlight color: warm sunrise/sunset, white midday
        sunColor = new Gradient
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(new Color(1f, 0.64f, 0.38f), 0.0f),  // Sunrise
                new GradientColorKey(Color.white, 0.25f),                 // Mid-morning
                new GradientColorKey(Color.white, 0.75f),                 // Afternoon
                new GradientColorKey(new Color(1f, 0.64f, 0.38f), 1.0f)   // Sunset
            },
            alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        };

        // --- Moonlight color: cool bluish white
        moonColor = new Gradient
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.7f, 0.8f, 1f), 0.0f),
                new GradientColorKey(new Color(0.7f, 0.8f, 1f), 1.0f)
            },
            alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        };

        // --- Sun intensity curve: fade in at sunrise, full at noon, fade out at sunset
        sunIntensity = new AnimationCurve(
            new Keyframe(0f, 0f),    // Midnight
            new Keyframe(0.20f, 1f), // Morning
            new Keyframe(0.5f, 1f),  // Noon
            new Keyframe(0.80f, 1f), // Evening
            new Keyframe(1f, 0f)     // Midnight
        );

        // --- Moon intensity curve: opposite of sun, softer at peak
        moonIntensity = new AnimationCurve(
            new Keyframe(0f, 1f),    // Midnight
            new Keyframe(0.20f, 0f), // Morning
            new Keyframe(0.5f, 0f),  // Noon
            new Keyframe(0.80f, 0f), // Evening
            new Keyframe(1f, 1f)     // Midnight
        );
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            timeOfDay += (Time.deltaTime / dayLength) * 24f;
            if (timeOfDay >= 24f) timeOfDay -= 24f;
        }

        UpdateSunAndMoonPosition();
        UpdateLighting();
    }

    private void UpdateSunAndMoonPosition()
    {
        if (sunPivot == null) return;

        float sunRot = ((timeOfDay - 6f) / 24f) * 360f + sunRotationOffset;
        sunPivot.localRotation = Quaternion.Euler(sunRot, 0f, 0f);

        if (moonLight != null)
            moonLight.transform.rotation = sunLight.transform.rotation * Quaternion.Euler(180f, 0f, 0f);
    }

    private void UpdateLighting()
    {
        float t = timeOfDay / 24f;

        if (sunLight != null)
        {
            sunLight.color = sunColor.Evaluate(t);
            sunLight.intensity = sunIntensity.Evaluate(t);
        }

        if (moonLight != null)
        {
            moonLight.color = moonColor.Evaluate(t);
            moonLight.intensity = moonIntensity.Evaluate(t);
        }
    }
}
