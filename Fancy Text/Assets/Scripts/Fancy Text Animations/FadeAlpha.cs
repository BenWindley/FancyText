using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeAlpha : FancyTextAnimation
{
    public override bool UpdateAnimation(float t, Transform letterTransform, TextMeshProUGUI text)
    {
        base.UpdateAnimation(t, letterTransform, text);

        var colour = text.color;
        colour.a = Mathf.Clamp(m_animationType == Type.Enter ? t * m_speed : 1 - t * m_speed, 0, 1);
        text.color = colour;

        return t * m_speed > 1.0f;
    }
}
 