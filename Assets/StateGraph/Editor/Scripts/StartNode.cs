using UnityEditor.Experimental.GraphView;
using System;

public class StartNode : BaseNode {

    public StartNode() {
        startPoint = true;
        endPoint = false;
    }

    /*
    public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type) {
        return ChildPort.Create<Edge>(orientation, direction, capacity, type);
    }
    */

    protected override Port CreateInputPort() {
        return null;
    }
}
