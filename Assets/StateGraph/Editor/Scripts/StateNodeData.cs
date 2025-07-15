using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

[Serializable]
public class StateNodeData : BaseNodeData {
    public string sceneName;
    public bool restartable;

    public List<string> ports;


    public StateNodeData(StateNode node) : base(node) {
        sceneName = node.SceneName;
        restartable = node.Restartable;
        ports = node.extensionContainer.Query<Port>().ToList().Select(port => port.portName).ToList();
    }

}
