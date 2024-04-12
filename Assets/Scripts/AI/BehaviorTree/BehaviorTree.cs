using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
	public Node rootNode;
	public Node.State treeState = Node.State.Running;

	public Node.State Update()
	{
		if (treeState == Node.State.Running)
		{
			treeState = rootNode.Update();
		}
		return treeState;
	}
}
