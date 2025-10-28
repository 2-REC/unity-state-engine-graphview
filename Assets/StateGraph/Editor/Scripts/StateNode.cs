using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class StateNode : BaseNode {

    private string _sceneName;
    public string SceneName {
        get => _sceneName;
        set {
            // TODO: make validity checks?
            _sceneName = value;
            _sceneTextField.SetValueWithoutNotify(value);
        }
    }

    private bool _restartable = false;
    public bool Restartable {
        get => _restartable;
        set {
            _restartable = value;
            _restartableCheckbox.SetValueWithoutNotify(value);
        }
    }

    private bool _leavable = false;
    public bool Leavable {
        get => _leavable;
        set {
            _leavable = value;
            _leavableCheckbox.SetValueWithoutNotify(value);
        }
    }

    private bool _isLevel = false;
    public bool IsLevel {
        get => _isLevel;
        set {
            _isLevel = value;
            _isLevelCheckbox.SetValueWithoutNotify(value);

            _sceneTextField.SetEnabled(!_isLevel);
            _sceneTextField.isPasswordField = _isLevel;
            _sceneTextField.maskChar = ' ';
        }
    }

    private readonly Label _titleLabel;
    private readonly TextField _nameTextField;
    private readonly Button _editNameButton;

    private readonly TextField _sceneTextField;
    private readonly Toggle _isLevelCheckbox;
    private readonly Toggle _restartableCheckbox;
    private readonly Toggle _leavableCheckbox;

    private readonly VisualElement _portContainer;


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
        _nameTextField.RegisterCallback<FocusOutEvent>(evt => {
            UpdateName("");
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
        _sceneTextField = new(string.Empty) {
            label = "Scene"
        };
        _sceneTextField.RegisterValueChangedCallback(evt => SceneName = evt.newValue);
        extensionContainer.Add(_sceneTextField);

        // 'level state' checkbox
        _isLevelCheckbox = new("Level state");
        _isLevelCheckbox.RegisterValueChangedCallback(evt => IsLevel = evt.newValue);
        extensionContainer.Add(_isLevelCheckbox);

        // 'restartable' checkbox
        _restartableCheckbox = new("Restartable");
        _restartableCheckbox.RegisterValueChangedCallback(evt => Restartable = evt.newValue);
        extensionContainer.Add(_restartableCheckbox);

        // 'leavable' checkbox
        _leavableCheckbox = new("Leavable");
        _leavableCheckbox.RegisterValueChangedCallback(evt => Leavable = evt.newValue);
        extensionContainer.Add(_leavableCheckbox);

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

        _nameTextField.SetValueWithoutNotify(name);
        _nameTextField.RemoveFromClassList("hidden");

        // hack to set focus ('Focus' doesn't work as element not visible yet)
        _nameTextField.schedule.Execute(schedule => { _nameTextField.Focus(); }).StartingIn(1);
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

    public List<BaseNode> GetChildrenNodes() {
        List<BaseNode> childrenNodes = new();

        _portContainer.Query<Port>().ForEach(p => {
            var transition = p.connections.FirstOrDefault();
            if (transition == null)
                return;
            var outputConnection = transition.input;
            if (outputConnection == null)
                return;

            if (outputConnection.node is BaseNode parent) {
                childrenNodes.Add(parent);
            }
        });

        return childrenNodes;
    }

}
