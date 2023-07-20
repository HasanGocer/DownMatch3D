using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectManager : MonoSingleton<ObjectManager>
{
    public int tempObjectCount;
    public bool isFree;

    public IEnumerator Move(GameObject moveObj, GameObject finishPos)
    {
        ObjectTouch objectTouch = moveObj.GetComponent<ObjectTouch>();
        float lerpCount = 0;
        Vector3 firstPos = moveObj.transform.position;

        while (lerpCount < 1)
        {
            lerpCount += Time.deltaTime * 7;
            moveObj.transform.position = Vector3.Lerp(firstPos, finishPos.transform.position, lerpCount);
            yield return null;
        }
    }
    public void OffTime(List<GameObject> objs)
    {
        StartCoroutine(Off(objs));
    }
    public IEnumerator Off(List<GameObject> objs)
    {
        if (objs.Count > 2)
        {
            List<ObjectID> objectIDs = new List<ObjectID>();

            Vibration.Vibrate(30);

            foreach (GameObject item in objs)
            {
                ObjectID objectID = item.GetComponent<ObjectID>();
                Vector3 finishPos;

                TaskUISystem.Instance.TaskDown(objectID.childCount);
                objectIDs.Add(objectID);

                if (Mathf.Abs(item.transform.position.x) == Mathf.Abs(item.transform.position.z))
                    finishPos = new Vector3(0.05f * (item.transform.position.x / Mathf.Abs(item.transform.position.x)), 0, 0.05f * (item.transform.position.z / Mathf.Abs(item.transform.position.z))) + item.transform.position;
                else if (Mathf.Abs(item.transform.position.x) > Mathf.Abs(item.transform.position.z))
                    finishPos = new Vector3(0.05f * (item.transform.position.x / Mathf.Abs(item.transform.position.x)), 0, 0) + item.transform.position;
                else
                    finishPos = new Vector3(0, 0, 0.05f * (item.transform.position.z / Mathf.Abs(item.transform.position.z))) + item.transform.position;

                item.transform.DOMove(finishPos, 0.2f);
                item.transform.DOScale(item.transform.localScale * 1.2f, 0.2f);

                PlacementSystem.Instance.floorBool[objectID.floorCount, objectID.roomCount] = false;
                PlacementSystem.Instance.apartment[objectID.floorCount, objectID.roomCount] = null;
            }

            yield return new WaitForSeconds(0.2f);

            foreach (ObjectID item in objectIDs)
            {
                DownSystem.Instance.AllDown(item);
                item.childs[item.childCount].SetActive(false);
                PlacementSystem.Instance.ObjectBack(item.gameObject);
                SoundSystem.Instance.CallBombSound();
                ParticalManager.Instance.CallObjectMergePartical(item.gameObject);
            }

            NewObjectSystem.Instance.CheckFloor();
        }
        isFree = true;
    }
}
