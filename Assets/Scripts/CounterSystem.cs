using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSystem : MonoSingleton<CounterSystem>
{
    public int counterCount;

    public void CounterStart()
    {
        counterCount = ItemData.Instance.field.counterCount;
    }
}
