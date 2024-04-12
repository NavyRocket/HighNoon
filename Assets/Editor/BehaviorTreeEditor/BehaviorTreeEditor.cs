using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeEditor : EditorWindow
{
	[SerializeField]
	private VisualTreeAsset m_VisualTreeAsset = default;

	[MenuItem("BehaviorTreeEditor/Editor ...")]
	public static void OpenWindow()
	{
		BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
		wnd.titleContent = new GUIContent("BehaviorTreeEditor");
	}

	public void CreateGUI()
	{
		// Each editor window contains a root VisualElement object
		VisualElement root = rootVisualElement;

		// Import UXML
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uxml");
		visualTree.CloneTree(root);

		// A stylesheet can be added to a VisualElement.
		// The style will be applied to the VisualElement and all of its children.
		var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uss");
		root.styleSheets.Add(styleSheet);
	}
}
