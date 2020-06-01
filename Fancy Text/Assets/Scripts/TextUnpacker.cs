using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextUnpacker : MonoBehaviour
{
    public List<GameObject> m_styles;

    public struct FancyTextExtract
    {
        public GameObject m_style;
        public string m_text;
    }

    public struct FancyTextBlock
    {
        public List<FancyTextExtract> m_textExtracts;
    }

    public List<FancyTextBlock> Unpack(TextAsset textAsset)
    {
        string textAssetText = textAsset.text;
        var text = textAssetText.Split(new[] { '\n'});

        List<FancyTextBlock> textBlocks = new List<FancyTextBlock>();
        FancyTextBlock textBlock = new FancyTextBlock();
        textBlock.m_textExtracts = new List<FancyTextExtract>();
        FancyTextExtract textExtract = new FancyTextExtract();

        foreach (string l in text)
        {
            string line = l;
            line = line.Replace("\r", "");

            if (line == "")
            {
                textExtract.m_text += "\n";
            }
            else if (line[0] == '<')
            {
                if (textExtract.m_text != null && textExtract.m_text != "")
                {
                    textBlock.m_textExtracts.Add(textExtract);

                    textExtract = new FancyTextExtract();
                }

                if (line.Contains("style"))
                {
                    line = line.Replace("style", "").Replace("<", "").Replace(">", "").Replace(" ", "");
                    textExtract.m_style = m_styles[int.Parse(line)];
                }
                else if(line.Contains("break"))
                {
                    textBlocks.Add(textBlock);

                    textBlock = new FancyTextBlock();
                    textBlock.m_textExtracts = new List<FancyTextExtract>();
                }
            }
            else
            {
                textExtract.m_text += line;
            }
        }

        return textBlocks;
    }
}
