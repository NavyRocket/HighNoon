using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class CombineMesh : MonoBehaviour
{
    void Awake()
    {
        Combine(transform);
    }

    void Combine(Transform parent)
    {
        MeshFilter meshFilter = parent.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            MeshCollider meshCollider = parent.gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }

        foreach (Transform child in parent)
        {
            Combine(child);
        }
    }
}
