using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstantAlphaTimer : FancyTextAnimation
{
    public override bool UpdateAnimation(float t, Transform letterTransform, TextMeshProUGUI text)
    {
        base.UpdateAnimation(t, letterTransform, text);

        var colour = text.color;

        switch (m_animationType)
        {
            case Type.Enter:
                if (t >= 1.0f)
                    colour.a = 1.0f;
                else
                    colour.a = 0.0f;
                break;
            case Type.Idle:
                colour.a = 1.0f;
                break;
            case Type.Exit:
                if (t >= 1.0f)
                    colour.a = 0.0f;
                else
                    colour.a = 1.0f;
                break;
        }

        text.color = colour;

        return true;
    }
}