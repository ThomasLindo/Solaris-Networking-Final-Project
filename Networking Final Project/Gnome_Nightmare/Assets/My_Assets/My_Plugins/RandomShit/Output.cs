using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
//using UnityEditor;
using System.IO;

public class Output : MonoBehaviour {
    [DllImport("Helloworld")]
    public static extern IntPtr Helloworld();
    [DllImport("Helloworld")]
    public static extern void Log(string CharName, string ItemName, string Value);
    [DllImport("Helloworld")]
    public static extern void ClearLog(string CharName, string ItemName);
    [DllImport("Helloworld")]
    public static extern IntPtr LoadDialogue(string CharName, string ItemName);
    [DllImport("Helloworld")]
    public static extern void SaveDialogue(string CharName, string ItemName, string Value);


    [System.Serializable]
    public class TextComponent
    {
        public string Path;
        public string Object;
        public string Text;
    }
    public TextComponent[] textComps;

    // Use this for initialization
    void Start () {
        ClearLog(gameObject.transform.name, gameObject.transform.ToString());
    }
	
	// Update is called once per frame
	void Update () {
        Log(gameObject.transform.name, gameObject.transform.ToString(), gameObject.transform.position.ToString());
        //foreach (TextComponent tc in textComps) { ; }
    }

    public void SaveText() {
        foreach (TextComponent tc in textComps) {
             WriteText(tc.Path, tc.Object, tc.Text);
             tc.Text = ReadText(tc.Path, tc.Object);
        }
    }


    public void WriteText(string v_Path, string v_Object, string v_Text) {
        SaveDialogue(v_Path, v_Object, v_Text);
    }

    public string ReadText(string v_Path, string v_Object) {
        string v_Text = Marshal.PtrToStringAnsi(LoadDialogue(v_Path, v_Object));
        if (v_Text == null) { v_Text = "ERROR.003 Could not read text."; }

        return v_Text;
    }
}

//[CustomEditor(typeof(Output))]
//public class OutputEditor : Editor {
//    public override void OnInspectorGUI() {
//        DrawDefaultInspector();
//        Output myScript = (Output)target;
//        if (GUILayout.Button("Save Text file")) {
//            myScript.SaveText();
//        }
//    }
//}