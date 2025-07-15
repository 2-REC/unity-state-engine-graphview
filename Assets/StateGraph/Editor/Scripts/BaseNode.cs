using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class BaseNode : Node {
    public string GUID;

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

    public virtual BaseNode GetNextNode() {
        Port outputPort = outputContainer.Query<Port>().First();
        if (outputPort == null)
            return null;

        var transition = outputPort.connections.FirstOrDefault();
        if (transition == null)
            return null;

        var outputConnection = transition.input;
        if (outputConnection == null)
            return null;

        return outputConnection.node as BaseNode;
    }

    // TODO: add 'GetInputNodes' method (mostly for checks)
    //...

}
