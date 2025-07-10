using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class StateNode : BaseNode {

    public bool restartable;

    public StateNode() {
        /* TODO: add CSS */
        Toggle restartableCheckbox = new("restartable");
        restartableCheckbox.RegisterValueChangedCallback(evt => restartable = evt.newValue);
        extensionContainer.Add(restartableCheckbox);

        var button = new Button(() => {
            AddChildPort();
        }) {
            text = "Add Child"
        };
        extensionContainer.Add(button);

        RefreshExpandedState();
        RefreshPorts();
    }

    public Port InstantiateChildPort() {
        return ChildPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(int));
    }

    public void AddChildPort(string portName="") {
        var port = InstantiateChildPort();

        /* TODO: keep global counter */
        //var nodeCount = extensionContainer.Query("connector").ToList().Count;
        var nodeCount = extensionContainer.Query<Port>().ToList().Count;
        port.portName = !string.IsNullOrEmpty(portName) ? portName : $"{nodeCount}";

        extensionContainer.Add(port);
        RefreshExpandedState();
        RefreshPorts();
    }
}
