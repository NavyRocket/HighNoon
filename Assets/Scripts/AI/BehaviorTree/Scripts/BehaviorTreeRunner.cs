using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
	public BehaviorTree _tree;

	// Start is called before the first frame update
	void Start()
	{
		_tree = _tree.Clone();
	}

	// Update is called once per frame
	void Update()
	{
		_tree.Update();
	}
}
