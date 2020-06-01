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
        m_fancyTextBlocks = m_textUnpacker.Unpack(m_text);

        m_textGenerator.Generate(m_fancyTextBlocks[m_textProgress++]);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_textProgress < m_fancyTextBlocks.Count)
        {
            m_textGenerator.RemoveLetters();
            m_textGenerator.Generate(m_fancyTextBlocks[m_textProgress++]);
        }
    }
}