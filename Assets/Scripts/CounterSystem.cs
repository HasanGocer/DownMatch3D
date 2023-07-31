using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSystem : MonoSingleton<CounterSystem>
{
    [HideInInspector] public int counterCount;

    public void CounterStart()
    {
        counterCount = ItemData.Instance.field.counterCount;
    }
}
