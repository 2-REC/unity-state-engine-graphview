using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class StateNode : BaseNode {

    public string sceneName;
    public bool restartable = false;

    private VisualElement _portContainer;


    public StateNode() {
        /* TODO: add CSS? */
        //extensionContainer.AddToClassList("state-node-extension-container");

        // 'scene' text field
        /* TODO: set default value */
        TextField textField = new(string.Empty) {
            label = "Scene"
        };
        textField.RegisterValueChangedCallback(evt => sceneName = evt.newValue);
        extensionContainer.Add(textField);

        // 'restartable' checkbox
        Toggle restartableCheckbox = new("Restartable");
        restartableCheckbox.RegisterValueChangedCallback(evt => restartable = evt.newValue);
        extensionContainer.Add(restartableCheckbox);

        // 'add child' button
        Button button = new(() => {
            AddChildPort();
        }) {
            text = "Add Child"
        };
        extensionContainer.Add(button);

        // ports container
        _portContainer = new VisualElement();
        _portContainer.style.flexDirection = FlexDirection.Row;
        extensionContainer.Add(_portContainer);

        RefreshExpandedState();
        RefreshPorts();
    }

    public Port InstantiateChildPort() {
        return ChildPort.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(int));
    }

    public void AddChildPort(string portName="") {
        Port port = InstantiateChildPort();

        if (string.IsNullOrEmpty(portName)) {
            int highest = -1;
            _portContainer.Query<Port>().ForEach(p => {
                int result = -1;
                try {
                    result = int.Parse(p.portName);
                } catch (FormatException) {}

                if (result > -1)
                    highest = Math.Max(result, highest);
            });
            port.portName = $"{highest + 1}";

        } else {
            port.portName = portName;
        }

        _portContainer.Add(port);
        RefreshExpandedState();
        RefreshPorts();
    }
}
