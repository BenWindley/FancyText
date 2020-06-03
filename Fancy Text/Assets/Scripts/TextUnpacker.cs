using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Definable
{
    public string m_name;
    public string m_replacement;

    public Definable(string name, string replacement)
    {
        m_name = "[" + name + "]";
        m_replacement = replacement;
    }
}

public class TextUnpacker : MonoBehaviour
{
    public static List<Definable> m_definableTerms = new List<Definable>();

    public struct FancyTextExtract
    {
        public GameObject m_style;
        public int m_size;
        public bool m_bold;
        public bool m_italic;
        public bool m_strikethrough;
        public bool m_underlined;
        public string m_audio;
        public string m_text;

        public FancyTextExtract(FancyTextExtract textExtract)
        {
            m_style = textExtract.m_style;
            m_size = textExtract.m_size;
            m_bold = textExtract.m_bold;
            m_italic = textExtract.m_italic;
            m_strikethrough = textExtract.m_strikethrough;
            m_underlined = textExtract.m_underlined;
            m_audio = textExtract.m_audio;
            m_text = "";
        }

        public FancyTextExtract(
            GameObject style, int size = 16, 
            bool bold = false, bool italic = false, 
            bool strikethrough = false, bool underlined = false, 
            string audio = "", string text = "")
        {
            m_style = style;
            m_size = size;
            m_bold = bold;
            m_italic = italic;
            m_strikethrough = strikethrough;
            m_underlined = underlined;
            m_audio = audio;
            m_text = text;
        }
    }

    public struct FancyTextBlock
    {
        public List<FancyTextExtract> m_textExtracts;
    }

    public static List<FancyTextBlock> Unpack(TextAsset textAsset)
    {
        List<GameObject> m_styles = new List<GameObject>();
        m_styles.AddRange(Resources.LoadAll<GameObject>("FancyText"));

        string textAssetText = textAsset.text;
        var text = textAssetText.Split(new[] { '\n'});

        List<FancyTextBlock> textBlocks = new List<FancyTextBlock>();
        FancyTextBlock textBlock = new FancyTextBlock();
        textBlock.m_textExtracts = new List<FancyTextExtract>();
        FancyTextExtract textExtract = new FancyTextExtract(m_styles[0], 32);

        foreach (string l in text)
        {
            string line = l;
            line = line.Replace("\r", "");

            foreach (Definable d in m_definableTerms)
            {
                line = line.Replace(d.m_name, d.m_replacement);
            }

            if (line == "")
            {
                textExtract.m_text += "\n";
            }
            else if (line[0] == '<')
            {
                if (textExtract.m_text != null && textExtract.m_text != "")
                {
                    textBlock.m_textExtracts.Add(textExtract);

                    textExtract = new FancyTextExtract(textBlock.m_textExtracts[textBlock.m_textExtracts.Count - 1]);
                }

                if (line.Contains("style"))
                {
                    line = line.Replace("style", "").Replace("<", "").Replace(">", "").Replace(" ", "");

                    foreach (GameObject style in m_styles)
                        if (style.name == line)
                        {
                            textExtract.m_style = style;
                            break;
                        }
                }
                else if (line.Contains("audio"))
                {
                    textExtract.m_audio = "event:/" + line.Replace("audio", "").Replace("<", "").Replace(">", "").Replace(" ", "");
                }
                else if (line.Contains("font"))
                {
                    line = line.Replace("font", "").Replace("<", "").Replace(">", "").Replace(" ", "");
                    textExtract.m_size = int.Parse(line);
                }
                else if (line.Contains("bold"))
                {
                    textExtract.m_bold = line.Contains("on");
                }
                else if (line.Contains("italic"))
                {
                    textExtract.m_italic = line.Contains("on");
                }
                else if (line.Contains("strikethrough"))
                {
                    textExtract.m_strikethrough = line.Contains("on");
                }
                else if (line.Contains("underline"))
                {
                    textExtract.m_underlined = line.Contains("on");
                }
                else if(line.Contains("break"))
                {
                    textBlocks.Add(textBlock);

                    textBlock = new FancyTextBlock();
                    textBlock.m_textExtracts = new List<FancyTextExtract>();
                    textExtract = new FancyTextExtract(textBlocks[textBlocks.Count - 1].m_textExtracts[textBlocks[textBlocks.Count - 1].m_textExtracts.Count - 1]);
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
