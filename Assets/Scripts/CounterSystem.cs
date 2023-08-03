using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterSystem : MonoSingleton<CounterSystem>
{
    int counterCount;
    [SerializeField] TMP_Text _counterText;

    public void CounterStart()
    {
        counterCount = ItemData.Instance.field.counterCount;
        _counterText.text = counterCount.ToString();
    }

    public int GetCounterCount()
    {
        return counterCount;
    }

    public void CounterDown()
    {
        counterCount--;
        _counterText.text = counterCount.ToString();
    }
}
