using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyTextBox : MonoBehaviour
{
    private TextGenerator m_textGenerator;
    private TextUnpacker m_textUnpacker;

    public TextAsset m_text;

    private List<TextUnpacker.FancyTextBlock> m_fancyTextBlocks;
    private int m_textProgress = 0;

    void Start()
    {
        m_textGenerator = GetComponent<TextGenerator>();
        m_textUnpacker = GetComponent<TextUnpacker>();
        TextUnpacker.m_definableTerms.Add(new Definable("name", "Ben"));
        m_fancyTextBlocks = TextUnpacker.Unpack(m_text);
        m_textGenerator.Generate(m_fancyTextBlocks[m_textProgress++]);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_textGenerator.LettersIdle() && m_textProgress < m_fancyTextBlocks.Count)
            {
                m_textGenerator.ForceStateLetters(FancyTextAnimation.Type.Exit);
                m_textGenerator.RemoveParagraphs();
                m_textGenerator.Generate(m_fancyTextBlocks[m_textProgress++]);
            }
            else
            {
                m_textGenerator.ForceStateLetters(FancyTextAnimation.Type.Idle);
            }
        }

    }
}
