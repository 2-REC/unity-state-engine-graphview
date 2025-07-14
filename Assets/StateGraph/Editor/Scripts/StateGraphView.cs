using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Text;
using System;

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

        serializeGraphElements += SerializeElements;
        canPasteSerializedData += IsValidSerializedData;
        unserializeAndPaste += UnserializeAndPaste;
    }

    public StartNode CreateStartNode() {
        var node = new StartNode {
            title = "START",
            name = "START"
        };
        node.capabilities &= ~Capabilities.Deletable;

        node.SetPosition(new Rect(100, 200, 100, 150));

        return node;
    }

    public EndNode CreateEndNode() {
        var node = new EndNode {
            title = "END",
            name = "END"
        };

        // NOTE: keep 'deletable' as allow more than 1 end node

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
            name = nodeName
        };
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

    string SerializeElements(IEnumerable<GraphElement> elements) {
        List<string> serializedData = new();

        foreach (var element in elements) {
            if (element is StateNode node) {
                StateNodeData nodeData = new(node);
                serializedData.Add(JsonUtility.ToJson(nodeData));
            }
        }

        StringBuilder stringBuilder = new();
        stringBuilder.AppendJoin(";", serializedData);
        return stringBuilder.ToString();
    }

    private bool IsValidSerializedData(string data) {
        try {
            List<string> strings = new(data.Split(";"));
            foreach (var str in strings) {
                // check can be casted to 'StateNodeData'
                JsonUtility.FromJson<StateNodeData>(str);
            }
        } catch {
            return false;
        }

        return true;
    }

    private void UnserializeAndPaste(string operationName, string data) {
        ClearSelection();
        foreach (var str in data.Split(";")) {
            var tempNode = JsonUtility.FromJson<StateNodeData>(str);

            StateNode stateNode = CreateStateNode(tempNode.name);
            stateNode.GUID = Guid.NewGuid().ToString();
            stateNode.sceneName = tempNode.sceneName;
            stateNode.restartable = tempNode.restartable;

            foreach (string portName in tempNode.ports) {
                stateNode.AddChildPort(portName);
            }

            stateNode.SetPosition(
                new Rect(
                    tempNode.position.x + 25,
                    tempNode.position.y + 25,
                    tempNode.position.width,
                    tempNode.position.height
                )
            );
            AddElement(stateNode);
            AddToSelection(stateNode);
        }
    }

}
