using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using System.Collections.Generic;


public class ChildPort : Port {

    protected ChildPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type)
        : base(portOrientation, portDirection, portCapacity, type) {

        Button deleteButton = new(() => {
            DeletePort();
        }) {
            tooltip = "Delete port"
        };
        contentContainer.Add(deleteButton);
    }

    /*
    public override void OnStartEdgeDragging() {
        base.OnStartEdgeDragging();
    }

    public override void OnStopEdgeDragging() {
        base.OnStopEdgeDragging();
    }
    */

    public new static Port Create<TEdge>(
        Orientation orientation,
        Direction direction,
        Capacity capacity,
        Type type
    ) where TEdge : Edge, new() {
        ChildPort port = new(orientation, direction, capacity, type);

        DefaultEdgeConnectorListener listener = new();
        port.m_EdgeConnector = new EdgeConnector<TEdge>(listener);
        port.AddManipulator(port.m_EdgeConnector);

        return port;
    }

    protected void DeletePort() {
        // disconnect and delete edges
        var enumer = connections.GetEnumerator();
        while (enumer.MoveNext()) {
            Edge edge = enumer.Current;
            edge.input.Disconnect(edge);
            Disconnect(edge);
            edge.RemoveFromHierarchy();

            // NOTE: Horrible hack to avoid modifying enumerable inside loop
            enumer = connections.GetEnumerator();
        }

        // delete port
        RemoveFromHierarchy();
    }

    private class DefaultEdgeConnectorListener : IEdgeConnectorListener {
        private GraphViewChange _graphViewChange;
        private List<Edge> _edgesToCreate;
        private List<GraphElement> _edgesToDelete;

        public DefaultEdgeConnectorListener() {
            _edgesToCreate = new List<Edge>();
            _edgesToDelete = new List<GraphElement>();
            _graphViewChange.edgesToCreate = _edgesToCreate;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position) {
            //Debug.Log("OnDropOutsidePort");
        }

        public void OnDrop(GraphView graphView, Edge edge) {
            _edgesToCreate.Clear();
            _edgesToCreate.Add(edge);
            _edgesToDelete.Clear();
            if (edge.input.capacity == Capacity.Single) {
                foreach (Edge connection in edge.input.connections) {
                    if (connection != edge)
                        _edgesToDelete.Add(connection);
                }
            }

            if (edge.output.capacity == Capacity.Single) {
                foreach (Edge connection in edge.output.connections) {
                    if (connection != edge)
                        _edgesToDelete.Add(connection);
                }
            }

            if (_edgesToDelete.Count > 0)
                graphView.DeleteElements(_edgesToDelete);
            List<Edge> edgesToCreate = _edgesToCreate;
            if (graphView.graphViewChanged != null)
                edgesToCreate = graphView.graphViewChanged(_graphViewChange).edgesToCreate;
            foreach (Edge edge1 in edgesToCreate) {
                graphView.AddElement(edge1);
                edge.input.Connect(edge1);
                edge.output.Connect(edge1);
            }
        }
    }
}
