using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StartSystem : MonoSingleton<StartSystem>
{
    [SerializeField] int _OPStarCount;
    [SerializeField] GameObject _finishPos;
    [SerializeField] GameObject firstPos, _parent;

    public void StartTime(int chidCount, Vector3 startPos)
    {
        StartCoroutine(StarTime(chidCount, startPos));
    }

    private IEnumerator StarTime(int chidCount, Vector3 startPos)
    {
        if (chidCount == 3)
        {
            for (int i = 0; i < 1; i++)
            {
                GameObject star = ObjectPool.Instance.GetPooledObjectAdd(_OPStarCount);
                star.transform.SetParent(_parent.transform);
                star.transform.localScale = new Vector3(1, 1, 1);
                star.transform.localRotation = Quaternion.identity;
                star.transform.localPosition = startPos;
                star.transform.localPosition += new Vector3(0, -150, 0);
                star.transform.DOMove(_finishPos.transform.position, 0.8f);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.7f);
            for (int i = 0; i < 1; i++)
            {
                MoneySystem.Instance.MoneyTextRevork(1);
                SoundSystem.Instance.CallStarSound();
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if (chidCount <= 5)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject star = ObjectPool.Instance.GetPooledObjectAdd(_OPStarCount);
                star.transform.SetParent(_parent.transform);
                star.transform.localScale = new Vector3(1, 1, 1);
                star.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                star.transform.localPosition = startPos;
                star.transform.DOMove(_finishPos.transform.position, 0.5f);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 3; i++)
            {
                MoneySystem.Instance.MoneyTextRevork(1);
                SoundSystem.Instance.CallStarSound();
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = 0; i < chidCount - 2; i++)
            {
                GameObject star = ObjectPool.Instance.GetPooledObjectAdd(_OPStarCount);
                star.transform.SetParent(_parent.transform);
                star.transform.localScale = new Vector3(1, 1, 1);
                star.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                star.transform.localPosition = startPos;
                star.transform.DOMove(_finishPos.transform.position, 0.5f);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.6f - chidCount * 0.1f);
            for (int i = 0; i < chidCount - 2; i++)
            {
                MoneySystem.Instance.MoneyTextRevork(1);
                SoundSystem.Instance.CallStarSound();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
