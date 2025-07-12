using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class StateNode : BaseNode {

    public string sceneName;
    public bool restartable = false;

    private VisualElement _portContainer;
    private Label _titleLabel;
    private TextField _nameTextField;
    private Button _editNameButton;


    public StateNode() {
        // TODO: add CSS?
        //extensionContainer.AddToClassList("state-node-extension-container");

        // get title label
        _titleLabel = titleContainer.Query<Label>().First();

        // rename text field (hidden)
        _nameTextField = new(string.Empty) {
            isDelayed = true
        };
        _nameTextField.RegisterValueChangedCallback(evt => {
            UpdateName(evt.newValue);
        });
        _nameTextField.AddToClassList("hidden");
        // insert right after title label
        titleContainer.Insert(_titleLabel.parent.IndexOf(_titleLabel) + 1, _nameTextField);

        // 'rename' button
        _editNameButton = new(() => {
            EditName();
        }) {
            text = "Rename"
        };
        titleButtonContainer.Add(_editNameButton);

        // 'scene' text field
        // TODO: set default value?
        TextField sceneTextField = new(string.Empty) {
            label = "Scene"
        };
        sceneTextField.RegisterValueChangedCallback(evt => sceneName = evt.newValue);
        extensionContainer.Add(sceneTextField);

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

    private void EditName() {
        _titleLabel.AddToClassList("hidden");
        _editNameButton.AddToClassList("hidden");

        _nameTextField.RemoveFromClassList("hidden");
    }

    private void UpdateName(string newName) {
        if (!string.IsNullOrEmpty(newName)) {
            title = newName;
            name = newName;
        }

        _nameTextField.AddToClassList("hidden");

        _titleLabel.RemoveFromClassList("hidden");
        _editNameButton.RemoveFromClassList("hidden");
    }

    public Port InstantiateChildPort() {
        return ChildPort.Create<Edge>(
            Orientation.Vertical,
            Direction.Output,
            Port.Capacity.Single,
            typeof(int)
        );
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
