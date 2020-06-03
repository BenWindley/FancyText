using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSort : MonoBehaviour
{
    public struct Word
    {
        public float m_wordWidth;
        public List<Letter> m_word;
        public GameObject m_wordObject;

        public Word(List<Letter> letters, Font font)
        {
            string word = "";

            m_wordWidth = 0.0f;
            m_word = letters;
            m_wordObject = new GameObject("");

            CharacterInfo characterInfo = new CharacterInfo();

            foreach (Letter l in letters)
            {
                word += l.m_text.text;

                font.RequestCharactersInTexture(l.m_text.text);
                font.GetCharacterInfo(l.m_text.text[0], out characterInfo);

                l.transform.SetParent(m_wordObject.transform, false);
                l.transform.localPosition = new Vector2(m_wordWidth, 0);

                m_wordWidth += Mathf.RoundToInt(characterInfo.advance * l.m_text.fontSize / font.fontSize);
            }

            m_wordObject.name = word;
        }
    }

    public struct Paragraph
    {
        public List<Word> m_paragraph;
        public GameObject m_paragraphObject;
        public float m_height;

        public Paragraph(List<Word> words, float lineSpacing, float initialSpacing, float maxLineWidth, float spaceWidth, Transform textTransform)
        {
            m_paragraph = words;
            m_paragraphObject = new GameObject("Paragraph");
            m_paragraphObject.transform.SetParent(textTransform, false);
            var rect = textTransform.GetComponent<RectTransform>();
            m_paragraphObject.transform.localPosition = new Vector2(-rect.sizeDelta.x, rect.sizeDelta.y) * rect.pivot;
            m_height = lineSpacing;

            float lineWidth = 0.0f;
            float lineHeight = initialSpacing;

            foreach(Word w in words)
            {
                w.m_wordObject.transform.SetParent(m_paragraphObject.transform, false);

                if(lineWidth + w.m_wordWidth > maxLineWidth)
                {
                    lineWidth = 0.0f;
                    lineHeight += lineSpacing;
                    m_height += lineSpacing;
                }

                w.m_wordObject.transform.localPosition = new Vector2(lineWidth, -lineHeight);

                lineWidth += w.m_wordWidth;
            }
        }
    }

    public enum TextAllignmentStyle
    {
        Left,
        Right
    }

    private List<Letter> m_specialLetters = new List<Letter>();

    public List<GameObject> OrderedSort(List<Letter> letters)
    {
        m_specialLetters.Clear();

        List<Paragraph> paragraphs = new List<Paragraph>();
        List<Letter> word = new List<Letter>();
        List<Word> words = new List<Word>();
        float spaceWidth = 0.0f;
        float heightSpacing = 0.0f;
        float letterHeightSpacing = 0.0f;
        Font font = new Font();

        CharacterInfo characterInfo = new CharacterInfo();

        foreach(Letter l in letters)
        {
            letterHeightSpacing = Mathf.Max(letterHeightSpacing, l.m_text.fontSize);
        }

        heightSpacing += letterHeightSpacing * 0.5f;

        for (int i = 0; i < letters.Count; ++i)
        {
            char letter = letters[i].m_text.text[0];

            font = letters[i].m_font;

            font.RequestCharactersInTexture(" ");
            font.GetCharacterInfo(' ', out characterInfo);

            spaceWidth = Mathf.RoundToInt(characterInfo.advance * letters[i].m_text.fontSize / font.fontSize);

            if (letter == '\n')
            {
                m_specialLetters.Add(letters[i]);

                if (!(i < letters.Count - 1))
                    break;

                if (word.Count > 0)
                {
                    words.Add(new Word(word, font));
                    word.Clear();
                }

                paragraphs.Add(new Paragraph(words, letterHeightSpacing, heightSpacing, GetComponent<RectTransform>().sizeDelta.x, spaceWidth, transform));
                heightSpacing += paragraphs[paragraphs.Count - 1].m_height;

                words.Clear();
            }
            else if ((char.IsLetterOrDigit(letter) || char.IsPunctuation(letter)) && !char.IsWhiteSpace(letter))
            {
                word.Add(letters[i]);
            }
            else
            {
                m_specialLetters.Add(letters[i]);

                word.Add(letters[i]);
                words.Add(new Word(word, font));
                word.Clear();
            }
        }

        words.Add(new Word(word, font));
        paragraphs.Add(new Paragraph(words, letterHeightSpacing, heightSpacing, GetComponent<RectTransform>().sizeDelta.x, spaceWidth, transform));

        DestroySpecialCharacters();

        List<GameObject> pObjects = new List<GameObject>();

        foreach(Paragraph p in paragraphs)
        {
            pObjects.Add(p.m_paragraphObject);
        }

        return pObjects;
    }

    public void DestroySpecialCharacters()
    {
        for (int i = 0; i < m_specialLetters.Count; ++i)
        {
            GameObject g = m_specialLetters[i].gameObject;
            g.transform.SetParent(null, false);
            
            DestroyImmediate(g);
        }
    }
}