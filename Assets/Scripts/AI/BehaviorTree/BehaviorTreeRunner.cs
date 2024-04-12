using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
	BehaviorTree tree;

	// Start is called before the first frame update
	void Start()
	{
		tree = ScriptableObject.CreateInstance<BehaviorTree>();
	}

	// Update is called once per frame
	void Update()
	{
		tree.Update();
	}
}
