using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GraphContainer : ScriptableObject {
    public BaseNodeData startNode;
    public BaseNodeData endNode;

    public List<StateNodeData> statesData = new List<StateNodeData>();
    public List<TransitionData> transitionsData = new List<TransitionData>();
}
