using UnityEngine;
using TMPro;

[System.Serializable]
public class LetterAnimation
{
    public AnimationCurve m_scale;
    public AnimationCurve m_positionX;
    public AnimationCurve m_positionY;
    public AnimationCurve m_rotationZ;
    public Gradient m_colour;
}

public class Letter : MonoBehaviour
{
    public enum State
    {
        None,
        Appearing,
        Idle,
        Disappearing
    }

    public LetterAnimation m_appearing;
    public LetterAnimation m_idle;
    public LetterAnimation m_disappearing;

    public float m_progress = 0.0f;

    public State m_currentState;
    public State m_nextState;

    public TextMeshProUGUI m_text;

    private void Update()
    {
        switch (m_currentState)
        {
            case State.None:
                m_currentState = m_nextState;
                break;

            case State.Appearing:
                m_progress += Time.deltaTime;

                UpdateLetter(m_appearing);

                if (m_progress > 1.0f)
                {
                    m_currentState = m_nextState;
                    m_progress -= 1.0f;
                }

                break;

            case State.Idle:
                m_progress += Time.deltaTime;

                UpdateLetter(m_idle);

                if (m_progress > 1.0f)
                {
                    m_currentState = m_nextState;
                    m_progress -= 1.0f;
                }

                break;

            case State.Disappearing:
                m_progress += Time.deltaTime;

                UpdateLetter(m_disappearing);

                if (m_progress > 1.0f)
                {
                    Remove();
                }

                break;
        }
    }

    private void UpdateLetter(LetterAnimation animation)
    {
        m_text.transform.localPosition = new Vector2(animation.m_positionX.Evaluate(m_progress), animation.m_positionY.Evaluate(m_progress));
        m_text.transform.localScale = Vector3.one * animation.m_scale.Evaluate(m_progress);
        m_text.color = animation.m_colour.Evaluate(m_progress);
        m_text.transform.rotation = Quaternion.Euler(0, 0, animation.m_rotationZ.Evaluate(m_progress));
    }

    private void Remove()
    {

    }
}
