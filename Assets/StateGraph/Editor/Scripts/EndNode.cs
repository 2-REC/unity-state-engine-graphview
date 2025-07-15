using UnityEditor.Experimental.GraphView;

public class EndNode : BaseNode {

    protected override Port CreateNextPort() {
        return null;
    }

    // not really necessary
    public override BaseNode GetNextNode() {
        return null;
    }

}
