using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{

    private string[] book_text;
    private bool status = false;
    private int word_index = 0;
    [SerializeField] string fileName;
    [SerializeField] float readingSpeed;
    TMP_Text textMeshDisplay;
    string[] file_name;
    private float waitTime = 0;
    private float timer = 0f;
    private Stopwatch stopwatch;
    
    char[] full_stop = { '.', '!', '?' };
    char[] half_stop = { ',', ':', ';', '-' };

    void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
        textMeshDisplay = gameObject.GetComponentInParent<TMP_Text>();
        file_name = fileName.Split('\\');
        book_text = ReadString();
        update_text(file_name[file_name.Length - 1]);
        waitTime = 60 / readingSpeed*1000;
    }

    void Update()
    {
        if (word_index == book_text.Length)
            CancelInvoke();
        textMeshDisplay.text = book_text[word_index];
        update_text(textMeshDisplay.text);
        
        if (stopwatch.ElapsedMilliseconds > waitTime)
        {
            if (book_text[word_index].IndexOfAny(full_stop) != -1)
            {
                System.Threading.Thread.Sleep((int)waitTime*2);
            } else if (book_text[word_index].IndexOfAny(half_stop) != -1)
            {
                System.Threading.Thread.Sleep((int)(waitTime * 1.2));
            }
            word_index++;
            stopwatch.Reset();
            stopwatch.Start();
        }
    }

    private void update_text(string word)
    {
        if (word.IndexOf("\r\n") > 0)
            word = word.Remove(word.IndexOf("\r\n"), 2);
        int index = get_colored_letter_index(word);
        
        get_richTextBox_location(index);
        AnimateVertexColors(index);
    }

    private void get_richTextBox_location(int index)
    {
        float X = textMeshDisplay.textInfo.characterInfo[index].topLeft.x;
        float X_next = textMeshDisplay.textInfo.characterInfo[index].topRight.x;
        float distanceBefore = X - textMeshDisplay.textInfo.characterInfo[0].topLeft.x;
        float letter_width = (X_next - X) / 2.0f;
        GetComponent<RectTransform>().anchoredPosition = new Vector3(-0.049f-(distanceBefore + letter_width) * 0.005f, -0.017f, -0.01f);
    }

    private string[] ReadString()
    {
        string path = "Assets/Resources/" + fileName + ".txt";
        StreamReader reader = new StreamReader(path);
        string textContent = reader.ReadToEnd();
        reader.Close();
        textContent = textContent.Replace("\r\n", " ");
        book_text = textContent.Split(' ');
        return book_text;
    }

    private int get_colored_letter_index(string word)
    {
        char[] chars_to_trim = { '.', ',', ':', ';', '!', '?', '-' };
        if (word.Trim(chars_to_trim).Length >= 18)
            return 5;
        else if (word.Trim(chars_to_trim).Length >= 14)
            return 4;
        else if (word.Trim(chars_to_trim).Length >= 10)
            return 3;
        else if (word.Trim(chars_to_trim).Length >= 6)
            return 2;
        else if (word.Trim(chars_to_trim).Length >= 2)
            return 1;
        else
            return 0;
    }

    private void AnimateVertexColors(int index)
    {
        TMP_TextInfo textInfo = textMeshDisplay.textInfo;
        Color32[] newVertexColors;
        Color32 c0 = textMeshDisplay.color;

        int materialIndex = textInfo.characterInfo[index].materialReferenceIndex;
        newVertexColors = textInfo.meshInfo[materialIndex].colors32;
        int vertexIndex = textInfo.characterInfo[index].vertexIndex;
        if (textInfo.characterInfo[index].isVisible)
        {
            c0 = Color.red;

            newVertexColors[vertexIndex + 0] = c0;
            newVertexColors[vertexIndex + 1] = c0;
            newVertexColors[vertexIndex + 2] = c0;
            newVertexColors[vertexIndex + 3] = c0;

            textMeshDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }
    
}

