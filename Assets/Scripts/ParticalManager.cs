using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalManager : MonoSingleton<ParticalManager>
{
    [SerializeField] int _OPFinishParticalCount;
    [SerializeField] int _OPObjectMergeParticalCount;

    public void CallObjectMergePartical(GameObject pos)
    {
        ObjectPool.Instance.GetPooledObjectAdd(_OPObjectMergeParticalCount, pos.transform.position);
    }
    public void CalLFinishPartical(GameObject pos)
    {
        ObjectPool.Instance.GetPooledObjectAdd(_OPFinishParticalCount, pos.transform.position);
    }
}
