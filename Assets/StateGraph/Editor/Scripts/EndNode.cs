using UnityEditor.Experimental.GraphView;
using System;

public class EndNode : BaseNode {

    public EndNode() {
        startPoint = false;
        endPoint = true;
    }

    /*
    public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type) {
        return ChildPort.Create<Edge>(orientation, direction, capacity, type);
    }
    */

    protected override Port CreateNextPort() {
        return null;
    }
}
