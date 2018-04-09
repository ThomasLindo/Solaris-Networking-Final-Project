using System.Collections.Generic;

[System.Serializable]
public class Dialogue { 
    public int NumberOfSentences;
    public string NameOfNPC;
    public List<string> FileName;
    public List<string> sentences;

    public void OnDestroy() {
        NumberOfSentences = 0;
        NameOfNPC = string.Empty;
        FileName.Clear();
        sentences.Clear();
    }
}

