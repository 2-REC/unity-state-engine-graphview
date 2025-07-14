using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class BaseNode : Node {
    public string GUID;

    /* TODO: needed? */
    public bool startPoint = false;
    public bool endPoint = false;

    public BaseNode() {
        GUID = Guid.NewGuid().ToString();

        // remove collapse button
        VisualElement collapseButton = null;
        foreach (VisualElement child in titleButtonContainer.Children()) {
            if (child.name == "collapse-button") {
                collapseButton = child;
            }
        }
        if (collapseButton != null)
            titleButtonContainer.Remove(collapseButton);

        CreateInputPort();
        CreateNextPort();
    }

    protected virtual Port CreateNextPort() {
        var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(int));
        port.portName = "Next";
        outputContainer.Add(port);

        RefreshExpandedState();
        RefreshPorts();

        return port;
    }

    protected virtual Port CreateInputPort() {
        var port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(int));
        port.portName = "Input";
        inputContainer.Add(port);

        RefreshExpandedState();
        RefreshPorts();

        return port;
    }
}
