using System;
using UnityEditor.Experimental.GraphView;

[Serializable]
public class TransitionData {
    public string startNodeGuid;
    public string portName;
    public string endNodeGuid;


    public TransitionData(Edge edge) {
        var inputNode = edge.input.node as BaseNode;
        var outputNode = edge.output.node as BaseNode;

        startNodeGuid = outputNode.GUID;
        portName = edge.output.portName;
        endNodeGuid = inputNode.GUID;
    }
}
