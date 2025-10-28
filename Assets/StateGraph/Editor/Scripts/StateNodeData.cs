using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

[Serializable]
public class StateNodeData : BaseNodeData {
    public string sceneName;
    public bool restartable;
    public bool leavable;
    public bool isLevel;

    public List<string> ports;


    public StateNodeData(StateNode node) : base(node) {
        sceneName = node.SceneName;
        restartable = node.Restartable;
        leavable = node.Leavable;
        isLevel = node.IsLevel;
        ports = node.extensionContainer.Query<Port>().ToList().Select(port => port.portName).ToList();
    }

}
