using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShakyText : FancyTextAnimation
{
    public float intensity = 1.0f;

    public override bool UpdateAnimation(float t, Transform letterTransform, TextMeshProUGUI text)
    {
        base.UpdateAnimation(t, letterTransform, text);

        letterTransform.localPosition = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * intensity;

        return true;
    }
}