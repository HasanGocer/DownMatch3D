using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectManager : MonoSingleton<ObjectManager>
{
    public bool firstSpace, secondSpace, thridSpace;
    public GameObject firstObject, secondObject, thridObject;
    public int tempObjectCount;
    public bool isFree;

    /* public void WrongItem()
     {
         isFree = true;
         LayerBack();
         if (firstSpace)
             WrongFirstObject();
         if (secondSpace)
             WrongSecondObject();
         if (thridSpace)
             WrongThristhObject();
         BoolOff();
         isFree = false;
     }*/
    /* public void ObjectCorrect()
     {
         MergeTime();
     }
     public void MergeTime()
     {
         isFree = true;
         LayerBack();
         BoolCheck();
         BoolOff();
         Vibration.Vibrate(30);

         List<GameObject> objs = new List<GameObject>();
         objs.Add(firstObject);
         objs.Add(secondObject);
         objs.Add(thridObject);

         ObjectOff();
         //CoinSystem.Instance.CoinStart();
         isFree = false;

         foreach (GameObject item in objs) item.SetActive(false);
         FinishSystem.Instance.FinishCheck();
     }*/
    /*  private void LayerBack()
      {
          if (firstSpace)
              firstObject.transform.GetChild(tempObjectCount).gameObject.layer = 0;
          if (secondSpace)
              secondObject.transform.GetChild(tempObjectCount).gameObject.layer = 0;
          if (thridSpace)
              thridObject.transform.GetChild(tempObjectCount).gameObject.layer = 0;
      }
      private void WrongFirstObject()
      {
          ObjectTouch objectTouch = firstObject.GetComponent<ObjectTouch>();
          GameObject tempObject = firstObject;
          firstObject = null;

          objectTouch.isSelected = false;
          tempObject.transform.SetParent(PlacementSystem.Instance.apartmentPos[objectTouch.objectID.floorCount, objectTouch.objectID.roomCount].transform);
          StartCoroutine(Move(tempObject, PlacementSystem.Instance.apartmentPos[objectTouch.objectID.floorCount, objectTouch.objectID.roomCount]));
          tempObject.transform.rotation = Quaternion.Euler(Vector3.zero);
          objectTouch.isFree = false;
      }
      private void WrongSecondObject()
      {
          ObjectTouch objectTouch = secondObject.GetComponent<ObjectTouch>();
          GameObject tempObject = secondObject;
          secondObject = null;

          objectTouch.isSelected = false;
          tempObject.transform.SetParent(PlacementSystem.Instance.apartmentPos[objectTouch.objectID.floorCount, objectTouch.objectID.roomCount].transform);
          StartCoroutine(Move(tempObject, PlacementSystem.Instance.apartmentPos[objectTouch.objectID.floorCount, objectTouch.objectID.roomCount]));
          tempObject.transform.rotation = Quaternion.Euler(Vector3.zero);
          objectTouch.isFree = false;
          Vibration.Vibrate(30);
      }
      private void WrongThristhObject()
      {
          ObjectTouch objectTouch = thridObject.GetComponent<ObjectTouch>();

          thridObject.transform.SetParent(PlacementSystem.Instance.apartmentPos[objectTouch.objectID.floorCount, objectTouch.objectID.roomCount].transform);
          thridObject.transform.DOMove(PlacementSystem.Instance.apartmentPos[objectTouch.objectID.floorCount, objectTouch.objectID.roomCount].transform.position, 0.3f);
          objectTouch.isFree = false;
          thridObject = null;
      }
      private void BoolCheck()
      {
          ObjectID firstObjectID = firstObject.GetComponent<ObjectID>();
          ObjectID secondObjectID = secondObject.GetComponent<ObjectID>();
          ObjectID thridObjectID = thridObject.GetComponent<ObjectID>();

          PlacementSystem.Instance.floorBool[firstObjectID.floorCount, firstObjectID.roomCount] = false;
          PlacementSystem.Instance.floorBool[secondObjectID.floorCount, secondObjectID.roomCount] = false;
          PlacementSystem.Instance.floorBool[thridObjectID.floorCount, thridObjectID.roomCount] = false;

          DownSystem.Instance.AllDown(firstObjectID, secondObjectID, thridObjectID);
      }
      private void ObjectOff()
      {
          firstObject = null;
          secondObject = null;
          thridObject = null;
      }
      private void BoolOff()
      {
          firstSpace = false;
          secondSpace = false;
          thridSpace = false;
      }*/
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
