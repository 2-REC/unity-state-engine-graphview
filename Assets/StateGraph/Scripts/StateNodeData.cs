using System;
using System.Collections.Generic;

[Serializable]
public class StateNodeData : BaseNodeData {
    public bool restartable;
    public List<string> ports;
}
