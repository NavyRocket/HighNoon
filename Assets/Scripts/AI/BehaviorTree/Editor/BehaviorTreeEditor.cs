using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeEditor : EditorWindow
{
	BehaviorTreeView _treeView;
	InspectorView _inspectorView;

	[MenuItem("BehaviorTree/Editor")]
	public static void OpenWindow()
	{
		BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
		wnd.titleContent = new GUIContent("BehaviorTreeEditor");
	}

	[OnOpenAsset]
	public static bool OnOpenAsset(int instanceID, int line)
	{
		if (Selection.activeObject is BehaviorTree)
		{
			OpenWindow();
			return true;
		}
		return false;
	}

	public void CreateGUI()
	{
		// Each editor window contains a root VisualElement object
		VisualElement root = rootVisualElement;

		// Import UXML
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Data/UIBuilder/BehaviorTreeEditor.uxml");
		visualTree.CloneTree(root);

		// A stylesheet can be added to a VisualElement.
		// The style will be applied to the VisualElement and all of its children.
		var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Data/UIBuilder/BehaviorTreeEditor.uss");
		root.styleSheets.Add(styleSheet);

		_treeView = root.Q<BehaviorTreeView>();
		_inspectorView = root.Q<InspectorView>();
		_treeView.onNodeSelected = OnNodeSelectionChanged;
		_treeView.focusable = true;

		OnSelectionChange();
	}

	private void OnSelectionChange()
	{
		BehaviorTree tree = Selection.activeObject as BehaviorTree;
		if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
		{
			_treeView.PopulateView(tree);
		}
	}

	private void OnNodeSelectionChanged(NodeView nodeView)
	{
		_inspectorView.UpdateSelection(nodeView);
	}
}
