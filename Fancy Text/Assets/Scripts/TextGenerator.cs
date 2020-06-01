using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGenerator : MonoBehaviour
{
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
                staggerCounter -= l.m_appearStagger;
                l.m_progress = staggerCounter;

                letters.Add(l);
            }
        }

        GetComponent<TextSort>().OrderedSort(letters);
    }

    public void RemoveLetters()
    {
        foreach(Letter l in GetComponentsInChildren<Letter>())
        {
            l.m_nextState = Letter.State.Disappearing;
        }
    }

    public void RemoveChildren(bool destroyImmediate = true)
    {
        int children = transform.childCount;

        for(int i = 0; i < children; ++i)
        {
            GameObject g = transform.GetChild(0).gameObject;
            g.transform.SetParent(null, false);

            if (destroyImmediate)
                DestroyImmediate(g);
            else
                Destroy(g);
        } 
    }
}
