using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class ID_Table : SerializedMonoBehaviour {

    public static ID_Table instance;
    private void Awake() { instance = this; DontDestroyOnLoad(this.gameObject); }

    [HideInInspector]
    public List<int> ItemList = Enumerable.Range(300, 200).ToList();

    [OdinSerialize]
    public Dictionary<int, GameObject> IDTable;

}