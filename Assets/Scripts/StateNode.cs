using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class StateNode : BaseNode {

    public StateNode() {
        /* TODO: add CSS */
        Toggle restartableCheckbox = new Toggle("restartable");
        extensionContainer.Add(restartableCheckbox);

        var button = new Button(() => {
            AddChildPort();
        });
        button.text = "Add Child";
        extensionContainer.Add(button);

        RefreshExpandedState();
        RefreshPorts();
    }

    public Port InstantiateChildPort() {
        return ChildPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(int));
    }

    private void AddChildPort() {
        var port = InstantiateChildPort();

        /* TODO: set better names... */
        var nodeCount = extensionContainer.Query("connector").ToList().Count;
        //port.portName = $"Child {nodeCount}";
        port.portName = $"{nodeCount}";

        extensionContainer.Add(port);
        RefreshExpandedState();
        RefreshPorts();
    }
}
