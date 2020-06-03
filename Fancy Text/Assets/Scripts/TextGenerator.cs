using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGenerator : MonoBehaviour
{
    private List<GameObject> m_paragraphs = new List<GameObject>();

    public void Generate(TextUnpacker.FancyTextBlock textBlock)
    {
        List<Letter> letters = new List<Letter>();

        float staggerCounter = 0.0f;

        foreach (var textExtract in textBlock.m_textExtracts)
        {
            for (int i = 0; i < textExtract.m_text.Length; ++i)
            {
                GameObject g = Instantiate(textExtract.m_style, transform);
                Letter l = g.GetComponent<Letter>();

                l.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = textExtract.m_text[i].ToString();
                l.name = textExtract.m_text[i].ToString();
                l.m_text.fontSize = textExtract.m_size;
                l.m_audio = textExtract.m_audio;

                if (textExtract.m_bold)
                    l.m_text.fontStyle |= TMPro.FontStyles.Bold;
                if (textExtract.m_italic)
                    l.m_text.fontStyle |= TMPro.FontStyles.Italic;
                if (textExtract.m_underlined)
                    l.m_text.fontStyle |= TMPro.FontStyles.Underline;
                if (textExtract.m_strikethrough)
                    l.m_text.fontStyle |= TMPro.FontStyles.Strikethrough;

                staggerCounter += l.m_appearStagger;
                l.m_wait = staggerCounter;

                letters.Add(l);
            }
        }

        m_paragraphs = GetComponent<TextSort>().OrderedSort(letters);
    }

    public bool LettersIdle()
    {
        foreach (Letter l in GetComponentsInChildren<Letter>())
        {
            if (l.m_currentState != FancyTextAnimation.Type.Idle)
                return false;
        }

        return true;
    }

    public void ForceStateLetters(FancyTextAnimation.Type state)
    {
        foreach (Letter l in GetComponentsInChildren<Letter>())
        {
            if (l.m_wait > 0)
                l.m_progress = l.m_wait % 1.0f;
            else
                l.m_progress = 0.0f;

            l.m_wait = 0.0f;
            l.m_currentState = state;
        }
    }

    public void RemoveParagraphs()
    {
        foreach(GameObject p in m_paragraphs)
            StartCoroutine(RemoveParagraph(p));
    }

    private IEnumerator RemoveParagraph(GameObject paragraph)
    {
        bool lettersFaded = false;

        while(!lettersFaded)
        {
            lettersFaded = (paragraph.GetComponentInChildren<Letter>() != null);

            yield return new WaitForSeconds(0.5f);
        }

        Destroy(paragraph);
    }
}
