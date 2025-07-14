using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSave {
    private StateGraphView _stateGraphView;

    private List<BaseNode> _nodes => _stateGraphView.nodes.ToList().Cast<BaseNode>().ToList();
    private List<Edge> _edges => _stateGraphView.edges.ToList();

    private GraphContainer _loadCache;


    public static GraphSave GetInstance(StateGraphView stateGraphView) {
        return new GraphSave {
            _stateGraphView = stateGraphView
        };
    }

    public void SaveGraph(string savePath) {
        Debug.Log($"Saving asset '{savePath}'");

        var graphContainer = ScriptableObject.CreateInstance<GraphContainer>();

        // start node (only 1)
        var startNode = _nodes.Where(node => node.startPoint).First();
        graphContainer.startNode = new BaseNodeData(startNode);

        // end node (only 1)
        var endNode = _nodes.Where(node => node.endPoint).First();
        graphContainer.endNode = new BaseNodeData(endNode);

        // transitions
        foreach (var edge in _edges) {
            graphContainer.transitionsData.Add(new TransitionData(edge));
        }

        // state nodes
        var nodes = _nodes.Where(node => !node.startPoint && !node.endPoint).ToList().Cast<StateNode>().ToList();
        foreach (var node in nodes) {
            graphContainer.statesData.Add(new StateNodeData(node));

        }

        AssetDatabase.CreateAsset(graphContainer, savePath);

        Debug.Log($"'{savePath}' saved");
    }
    public void LoadGraph(string loadPath) {
        Debug.Log($"Loading asset '{loadPath}'");

        _loadCache = (GraphContainer)AssetDatabase.LoadAssetAtPath(loadPath, typeof(GraphContainer));
        if (_loadCache == null) {
            EditorUtility.DisplayDialog("File not found", "Graph asset file not found!", "OK");
            return;
        }

        Debug.Log($"_loadCache: {_loadCache}");

        ClearGraph();
        CreateNodes();
        CreateTransitions();

        Debug.Log($"'{loadPath}' loaded");
    }

    private void ClearGraph() {
        _stateGraphView.DeleteElements(_edges);
        _stateGraphView.DeleteElements(_nodes);
    }

    private void CreateNodes() {
        // start node
        var startNode = _stateGraphView.CreateStartNode();
        startNode.GUID = _loadCache.startNode.guid;
        startNode.name = _loadCache.startNode.name;
        startNode.SetPosition(_loadCache.startNode.position);
        _stateGraphView.AddElement(startNode);

        // end node
        var endNode = _stateGraphView.CreateEndNode();
        endNode.GUID = _loadCache.endNode.guid;
        endNode.name = _loadCache.endNode.name;
        endNode.SetPosition(_loadCache.endNode.position);
        _stateGraphView.AddElement(endNode);

        // state nodes
        foreach (var cacheNode in _loadCache.statesData) {
            var stateNode = _stateGraphView.CreateStateNode(cacheNode.name);
            stateNode.GUID = cacheNode.guid;
            stateNode.sceneName = cacheNode.sceneName;
            stateNode.restartable = cacheNode.restartable;

            foreach (string portName in cacheNode.ports) {
                stateNode.AddChildPort(portName);
            }

            stateNode.SetPosition(cacheNode.position);
            _stateGraphView.AddElement(stateNode);
        }
    }

    private void CreateTransitions() {
        // transitions (+ports)
        foreach (var cacheEdge in _loadCache.transitionsData) {
            // get input node
            var inputNode = _nodes.Where(n => n.GUID == cacheEdge.startNodeGuid).First();
            Debug.Log($"inputNode: {inputNode.name}");

            // get input port
            Port inputPort = cacheEdge.portName == "Next" ?
                inputNode.outputContainer.Query<Port>().First()
                : inputNode.extensionContainer.Query<Port>().ToList().Where(x => x.portName == cacheEdge.portName).First();
            Debug.Log($"inputPort: {inputPort.portName}");

            // get output node
            var outputNode = _nodes.Where(n => n.GUID == cacheEdge.endNodeGuid).First();
            Debug.Log($"outputNode: {outputNode.name}");

            // get input port
            Port outputPort = outputNode.inputContainer.Query<Port>().First();
            Debug.Log($"outputPort: {outputPort.portName}");


            // create transition
            /* NOTE: not as in tutorial! */
            Edge edge = outputPort.ConnectTo(inputPort);
            _stateGraphView.AddElement(edge);
        }
    }

}
