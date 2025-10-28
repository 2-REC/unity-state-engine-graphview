using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GraphContainer : ScriptableObject {
    public BaseNodeData startNode;

    public List<StateNodeData> statesData = new();
    public List<TransitionData> transitionsData = new();
}
