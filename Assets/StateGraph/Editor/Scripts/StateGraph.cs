using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class StateGraph : EditorWindow {

    private StateGraphView _graphView;

    private static int _count = 0;
    private string _savePath;
    private string _exportPath;

    [MenuItem("Graph/State Graph")]
    public static void OpenStateGraphWindow() {
        var window = GetWindow<StateGraph>();
        window.titleContent = new GUIContent("State Graph");
    }

    private void OnEnable() {
        BuildToolbar();
        BuildToolbar2();
        BuildGraphView();
        //BuildMiniMap();
    }

    private void OnDisable() {
        rootVisualElement.Remove(_graphView);

    }

    private void BuildGraphView() {
        _graphView = new StateGraphView {
            name = "State Graph"
        };

        // using 'StretchToParentSize' overlaps the toolbars
        _graphView.style.flexGrow = 1;
        rootVisualElement.Add(_graphView);

    }

    private void BuildToolbar() {
        Toolbar toolbar = new();

        // 'Save' button
        Button saveButton = new(() => {
            SaveGraph();
        }) {
            text = "Save Graph"
        };
        toolbar.Add(saveButton);

        // 'Load' button
        Button loadButton = new(() => {
            LoadGraph();
        }) {
            text = "Load Graph"
        };
        toolbar.Add(loadButton);

        // 'Generate' button
        Button generateButton = new(() => {
            GenerateGraph();
        }) {
            text = "Generate Graph"
        };
        toolbar.Add(generateButton);

        rootVisualElement.Add(toolbar);
    }

    // TODO: rename!
    private void BuildToolbar2() {
        Toolbar toolbar = new();

        // TODO: move to other toolbar
        // 'Create Node' button
        Button nodeCreateButton = new(() => {
            // TODO: new state default name? (should be unique)
            ++_count;
            _graphView.CreateNode($"STATE_{_count}");
            //_graphView.CreateNode("<NEW_STATE>");
        }) {
            text = "Create Node"
        };
        toolbar.Add(nodeCreateButton);

        // 'Reframe' button
        Button reframeButton = new(() => {
            _graphView.FrameAll();
        }) {
            text = "Reframe"
        };
        toolbar.Add(reframeButton);

        rootVisualElement.Add(toolbar);
    }

    /*
    private void BuildMiniMap() {
        MiniMap miniMap = new() {
            anchored = true
        };
        // ????
        Debug.Log($"HEIGHT: {_graphView.worldBound.height}");
        // TODO: get offset due to toolbar/buttons
        miniMap.SetPosition(new Rect(8, 24, 200, 200));
        _graphView.Add(miniMap);

        // hide Label
        miniMap.Query<Label>().First().AddToClassList("hidden");
    }
    */

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

        string savePath = EditorUtility.SaveFilePanelInProject("Save Graph", defaultName, "asset", "Save graph as...", directory);
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

    private void GenerateGraph() {
        // get from previous export if any
        string defaultName, directory;
        if (string.IsNullOrEmpty(_exportPath)) {
            defaultName = _graphView.name;
            directory = Application.dataPath;
        } else {
            defaultName = Path.GetFileName(_exportPath);
            directory = Path.GetDirectoryName(_exportPath);
        }

        string exportPath = EditorUtility.SaveFilePanelInProject(
            "Export Graph",
            defaultName,
            "xml",
            "Export graph as...",
            directory
        );
        if (string.IsNullOrEmpty(exportPath)) {
            return;
        }

        var graphSave = GraphSave.GetInstance(_graphView);
        graphSave.ExportGraph(exportPath);

    }

}
