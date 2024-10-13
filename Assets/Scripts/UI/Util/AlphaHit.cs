using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(CanvasRenderer))]
//[RequireComponent(typeof(Image))]
//[RequireComponent(typeof(Button))]

public class AlphaHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = .1f;
    }
}
