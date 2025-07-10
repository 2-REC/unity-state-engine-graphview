using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

public class StateGraph : EditorWindow {

    private StateGraphView _graphView;

    private string _savePath;

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

        // 'Save' button
        var saveButton = new Button(() => {
            SaveGraph();
        });
        saveButton.text = "Save Graph";
        toolbar.Add(saveButton);

        // 'Load' button
        var loadButton = new Button(() => {
            LoadGraph();
        });
        loadButton.text = "Load Graph";
        toolbar.Add(loadButton);

        // 'Create Node' button
        var nodeCreateButton = new Button(() => {
            _graphView.CreateNode("STATE");
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

    private void SaveGraph() {
        // get from previous save if any
        string defaultName, directory;
        if (string.IsNullOrEmpty(_savePath)) {
            defaultName = _graphView.name;
            directory = Application.dataPath;
        } else {
            defaultName = Path.GetFileName(_savePath);
            directory = Path.GetDirectoryName(_savePath);
        }
        Debug.Log($"defaultName: {defaultName}");
        Debug.Log($"directory: {directory}");

        string savePath = EditorUtility.SaveFilePanelInProject("Save Graph", defaultName, "asset", "Save graph as...", directory);
        Debug.Log($"save path: {savePath}");
        if (string.IsNullOrEmpty(savePath)) {
            return;
        }

        var graphSave = GraphSave.GetInstance(_graphView);
        graphSave.SaveGraph(savePath);
        /* TODO: update graph name? */
        _savePath = savePath;
    }
    private void LoadGraph() {
        // get from previous load if any
        string directory;
        if (string.IsNullOrEmpty(_savePath)) {
            directory = Application.dataPath;
        } else {
            directory = Path.GetDirectoryName(_savePath);
        }
        Debug.Log($"directory: {directory}");

        var loadPath = EditorUtility.OpenFilePanel("Load Graph", directory, "asset");
        if (string.IsNullOrEmpty(loadPath)) {
            return;
        }
        // make path internal to project
        loadPath = loadPath.Replace(Application.dataPath, "Assets");

        var graphSave = GraphSave.GetInstance(_graphView);
        graphSave.LoadGraph(loadPath);
        /* TODO: update graph name? */
        _savePath = loadPath;
    }
}
