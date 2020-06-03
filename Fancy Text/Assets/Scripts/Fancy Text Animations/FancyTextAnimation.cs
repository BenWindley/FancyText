using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FancyTextAnimation : MonoBehaviour
{
    public enum Type
    {
        Enter,
        Idle,
        Exit
    }

    public Type m_animationType;
    public float m_speed = 1.0f;

    public virtual bool UpdateAnimation(float t, Transform letterTransform, TextMeshProUGUI text)
    {
        return false;
    }
}
