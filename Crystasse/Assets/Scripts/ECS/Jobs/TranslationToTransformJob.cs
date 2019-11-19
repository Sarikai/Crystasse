using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

public struct TranslationToTransformJob : IJobForEach<ID, Translation>
{
    public int TransformID;
    public TransformAccess Transform;

    public void Execute([ReadOnly] ref ID c0, [ReadOnly] ref Translation c1)
    {
        if(c0.Value == TransformID)
            Transform.position = c1.Value;
    }
}
