using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class StateGraphView : GraphView {

    private readonly Vector2 defaultNodeSize = new(150, 200);

    public StateGraphView() {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        var grid = new GridBackground();
        //grid.name = "Grid";
        Insert(0, grid);
        grid.StretchToParentSize();

        styleSheets.Add(Resources.Load<StyleSheet>("StateGraph"));

        var startNode = CreateStartNode();
        AddElement(startNode);

        var endNode = CreateEndNode();
        AddElement(endNode);
    }

    //private StateNode CreateStartNode() {
    /* TODO: add name as param (same as title?) */
    public StartNode CreateStartNode() {
        /*
        var node = new StateNode {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            entryPoint = true
        };

        var port = CreatePort(node, Direction.Output);
        */
        //var node = new Node {
        var node = new StartNode {
            title = "START",
            name = "START",
            GUID = Guid.NewGuid().ToString(),
            /*entryPoint = true*/
        };
        node.capabilities &= ~Capabilities.Deletable;
        ////////
        /* TODO: make 'CreateNextPort' method in 'BaseNode' */
        /*
        var port = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(int));
        port.portName = "Next";
        node.outputContainer.Add(port);

        node.RefreshExpandedState();
        node.RefreshPorts();
        */
        ////////

        node.SetPosition(new Rect(100, 200, 100, 150));

        return node;
    }

    public EndNode CreateEndNode() {
        var node = new EndNode {
            title = "END",
            name = "END",
            GUID = Guid.NewGuid().ToString()
        };

        node.SetPosition(new Rect(500, 200, 100, 150));

        return node;
    }

    public void CreateNode(string nodeName) {
        Node node = CreateStateNode(nodeName);

        Vector2 worldPosition = new(
            contentContainer.worldBound.width / 2 + contentContainer.worldBound.x,
            contentContainer.worldBound.height / 2 + contentContainer.worldBound.y
        );

        Vector2 localPosition = contentViewContainer.WorldToLocal(worldPosition);
        node.SetPosition(new Rect(localPosition - defaultNodeSize / 2, defaultNodeSize));

        AddElement(node);
    }

    public StateNode CreateStateNode(string nodeName) {
        var node = new StateNode {
            title = nodeName,
            name = nodeName,
            GUID = Guid.NewGuid().ToString()
        };

        /*
        var inputPort = CreateInputPort(node);
        node.inputContainer.Add(inputPort);

        // TODO: add 'Next' port
        // ...

        var button = new Button(() => {
            AddChildPort(node);
        });
        button.text = "Add Child";
        node.titleContainer.Add(button);

        node.RefreshExpandedState();
        node.RefreshPorts();
        */

        //node.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

        return node;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdpater) {
        var compatiblePorts = new List<Port>();

        ports.ForEach(port => {
            // TODO: allow self as next or child?
            //if (startPort == port || startPort.node == port.node) {
            if (startPort == port)
                return;
            if (startPort.direction == port.direction)
                return;

            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    /*
    private Port CreateInputPort(StateNode node, string portName="Input") {
        var port = node.InstantiateInputPort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(int));
        port.portName = portName;
        return port;
    }
    */

    /*
    private Port CreateChildPort(StateNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single) {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(int));
    }
    */

    /*
    private void AddChildPort(StateNode node) {
        var port = CreateChildPort(node, Direction.Output);
        //var port = new ChildPort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(int));

        // TODO: 'extensionContainer' or 'outputContainer'?
        var nodeCount = node.extensionContainer.Query("connector").ToList().Count;
        port.portName = $"Child {nodeCount}";

        node.extensionContainer.Add(port);
        node.RefreshExpandedState();
        node.RefreshPorts();
    }
    */
}
