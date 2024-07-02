using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioClipNode : ActionNode
{
    public AudioClip clip;

    protected override void OnStart()
    {
        blackboard.audioSource.clip = clip;
        blackboard.audioSource.Play();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
