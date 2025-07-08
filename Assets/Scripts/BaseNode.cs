using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class BaseNode : Node {
    public string GUID;

    /* TODO: needed? */
    public bool entryPoint = false;
    public bool exitPoint = false;

    public BaseNode() {
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

    /*
    public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type) {
        return Port.Create<Edge>(orientation, direction, capacity, type);
    }
    */

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
