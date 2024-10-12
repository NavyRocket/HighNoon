using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartycleSystemDisactivate : MonoBehaviour
{
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps.isPlaying)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ps.Play();
    }
}
