using UnityEditor.Experimental.GraphView;

public class StartNode : BaseNode {

    public StartNode() {
        startPoint = true;
        endPoint = false;
    }

    protected override Port CreateInputPort() {
        return null;
    }
}
