using UnityEditor.Experimental.GraphView;

public class EndNode : BaseNode {

    public EndNode() {
        startPoint = false;
        endPoint = true;
    }

    protected override Port CreateNextPort() {
        return null;
    }
}
