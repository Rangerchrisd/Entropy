using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DialogueLoader : MonoBehaviour
{
    public string path = "";
    public bool ready;
    public Dialogue dialogue;
    private void OnValidate()
    {
        if (path.Length != 0 && ready&&dialogue)
        {
            Debug.Log(path);
            //dialogue.dialogueLines = new string[countLines("Assets\\"+path)];
            StreamReader myReader = new StreamReader("Assets\\"+path);
            string reading = myReader.ReadLine();
            int i = 0;
            dialogue.dialogueLines.Clear();
            while (!myReader.EndOfStream)
            {
                dialogue.dialogueLines.Add(reading);
                i++;
                reading = myReader.ReadLine();
            }
            dialogue.dialogueLines[i] = reading;
            myReader.Close();
            ready = false;
        }
    }
    public int countLines(string path)
    {
        StreamReader myReader = new StreamReader(path);
        int i = 0;
        while (!myReader.EndOfStream)
        {
            i++;
            myReader.ReadLine();
        }
        myReader.Close();
        return i;
    }
}
