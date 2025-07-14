using System;
using UnityEngine;

[Serializable]
public class BaseNodeData {
    public string guid;
    public string name;

    public Rect position;


    public BaseNodeData(BaseNode node) {
        guid = node.GUID;
        name = node.name;

        position = node.GetPosition();
    }

}
