using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstantAlpha : FancyTextAnimation
{
    public override bool UpdateAnimation(float t, Transform letterTransform, TextMeshProUGUI text)
    {
        base.UpdateAnimation(t, letterTransform, text);

        var colour = text.color;
        colour.a = m_animationType == Type.Exit ? 0.0f : 1.0f;
        text.color = colour;

        return true;
    }
}