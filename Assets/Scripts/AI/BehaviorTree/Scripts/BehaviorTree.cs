using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
	public Node rootNode;
	public Node.State treeState = Node.State.Running;
	public List<Node> nodes = new List<Node>();

	public Node.State Update()
	{
		if (treeState == Node.State.Running)
		{
			treeState = rootNode.Update();
		}

		return treeState;
	}

	public Node CreateNode(System.Type type)
	{
		Node node = ScriptableObject.CreateInstance(type) as Node;
		node.name = type.Name;
		node.guid = GUID.Generate().ToString();
		nodes.Add(node);

		AssetDatabase.AddObjectToAsset(node, this);
		AssetDatabase.SaveAssets();

		return node;
	}

	public void DeleteNode(Node node)
	{
		nodes.Remove(node);
		AssetDatabase.RemoveObjectFromAsset(node);
		AssetDatabase.SaveAssets();
	}

	public void AddChild(Node parent, Node child)
	{
		CompositeNode composite = parent as CompositeNode;
		if (composite)
		{
            composite.children.Add(child);
		}

		DecoratorNode decorator = parent as DecoratorNode;
		if (decorator)
		{
			decorator.child = child;
		}
	}

	public void RemoveChild(Node parent, Node child)
	{

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Remove(child);
        }

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = null;
        }
    }

	public List<Node> GetChildren(Node parent)
	{
		List<Node> children = new List<Node>();

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
			return composite.children;
        }

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            children.Add(decorator.child);
        }

		return children;
    }
}
