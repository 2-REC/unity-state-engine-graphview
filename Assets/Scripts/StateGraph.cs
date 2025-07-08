using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StateGraph : EditorWindow {

    private StateGraphView _graphView;

    [MenuItem("Graph/State Graph")]
    public static void OpenStateGraphWindow() {
        var window = GetWindow<StateGraph>();
        window.titleContent = new GUIContent("State Graph");
    }

    private void OnEnable() {
        BuildGraphView();
        BuildToolbar();
    }

    private void OnDisable() {
        rootVisualElement.Remove(_graphView);

    }

    private void BuildToolbar() {
        var toolbar = new UnityEditor.UIElements.Toolbar();

        var nodeCreateButton = new Button(() => {
            _graphView.CreateNode("New Node");
        });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void BuildGraphView() {
        _graphView = new StateGraphView {
            name = "State Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);

    }

}
