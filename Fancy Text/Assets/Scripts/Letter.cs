using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    private List<FancyTextAnimation> m_enterAnimations = new List<FancyTextAnimation>();
    private List<FancyTextAnimation> m_idleAnimations  = new List<FancyTextAnimation>();
    private List<FancyTextAnimation> m_exitAnimations  = new List<FancyTextAnimation>();

    public float m_progress = 0.0f;
    public float m_wait = 0.0f;
    public float m_appearStagger = 0.1f;

    public FancyTextAnimation.Type m_currentState;

    public Transform m_textTransform;
    public TextMeshProUGUI m_text;
    public Font m_font;
    public string m_audio;

    private void Start()
    {
        foreach(FancyTextAnimation a in GetComponents<FancyTextAnimation>())
        {
            switch (a.m_animationType)
            {
                case FancyTextAnimation.Type.Enter:
                    m_enterAnimations.Add(a);
                    break;
                case FancyTextAnimation.Type.Idle:
                    m_idleAnimations.Add(a);
                    break;
                case FancyTextAnimation.Type.Exit:
                    m_exitAnimations.Add(a);
                    break;
            }
        }

        foreach (FancyTextAnimation a in m_enterAnimations)
        {
            a.UpdateAnimation(0, m_textTransform, m_text);
        }
    }

    private void LateUpdate()
    {
        if ((m_wait -= Time.deltaTime) > 0.0f)
            return;

        bool finishedState = true;
        m_progress += Time.deltaTime;

        switch (m_currentState)
        {
            case FancyTextAnimation.Type.Enter:
                foreach(FancyTextAnimation a in m_enterAnimations)
                {
                    finishedState &= a.UpdateAnimation(m_progress, m_textTransform, m_text);
                }

                if(finishedState)
                {
                    m_progress = 0.0f;
                    FModUtility.PlayOneShot(m_audio);
                    m_currentState = FancyTextAnimation.Type.Idle;
                }

                break;
            case FancyTextAnimation.Type.Idle:
                foreach (FancyTextAnimation a in m_idleAnimations)
                {
                    finishedState &= a.UpdateAnimation(m_progress, m_textTransform, m_text);
                }

                if (finishedState)
                {
                    m_progress = 0.0f;
                }

                break;
            case FancyTextAnimation.Type.Exit:
                foreach (FancyTextAnimation a in m_exitAnimations)
                {
                    finishedState &= a.UpdateAnimation(m_progress, m_textTransform, m_text);
                }

                if (finishedState)
                {
                    Remove();
                }

                break;
        }
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}
