using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectTouch : MonoBehaviour
{
    public ObjectID objectID;
    public bool isFree, isSelected, isDown;
    Vector3 firstPos;

    private void OnMouseDown()
    {
        CameraMove.Instance.focusObject = gameObject;
        CameraMove.Instance.isObjectTouch = true;
    }

    public void Touch()
    {
        ObjectManager objectManager = ObjectManager.Instance;
        CameraMove.Instance.isObjectTouch = false;

        if (GameManager.Instance.gameStat == GameManager.GameStat.start && objectManager.isFree && !isFree)
        {
            firstPos = Input.GetTouch(0).position;
            isFree = true;
            objectManager.isFree = false;
            List<GameObject> objs = new List<GameObject>();
            objs.Add(gameObject);
            int childCount = objectID.childCount;

            UpTime(ref objs, objectID.floorCount + 1, objectID.roomCount, childCount);
            DownTime(ref objs, objectID.floorCount - 1, objectID.roomCount, childCount);
            LeftTime(ref objs, objectID.floorCount, objectID.roomCount - 1, childCount);
            RightTime(ref objs, objectID.floorCount, objectID.roomCount + 1, childCount);
            if (objs.Count >= 3)
                StartSystem.Instance.StartTime(objs.Count, firstPos);
            objectManager.OffTime(objs);
            isFree = false;
        }

    }
    private void UpTime(ref List<GameObject> Objs, int floorCount, int roomCount, int childCount)
    {
        ItemData itemData = ItemData.Instance;
        PlacementSystem placementSystem = PlacementSystem.Instance;

        if (floorCount != itemData.field.floorCount)
            if (placementSystem.floorBool[floorCount, roomCount])
            {
                if (CheckObjs(ref Objs, placementSystem.apartment[floorCount, roomCount]))
                    if (placementSystem.apartment[floorCount, roomCount].GetComponent<ObjectID>().childCount == childCount)
                    {
                        Objs.Add(placementSystem.apartment[floorCount, roomCount]);

                        if (floorCount + 1 != itemData.field.floorCount)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);

                        if (roomCount - 1 != -1)
                            LeftTime(ref Objs, floorCount, roomCount - 1, childCount);
                        else
                            LeftTime(ref Objs, floorCount, placementSystem.ySizeDis - 1, childCount);

                        if (roomCount + 1 != placementSystem.ySizeDis)
                            RightTime(ref Objs, floorCount, roomCount + 1, childCount);
                        else
                            RightTime(ref Objs, floorCount, 0, childCount);
                    }
                    else { }
                else
                {
                    print("HG");
                }
                /*   else
                  {
                      if (floorCount + 1 != itemData.field.floorCount)
                      {
                          UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                          if (roomCount - 1 != -1)
                              LeftTime(ref Objs, floorCount, roomCount - 1, childCount);
                          else
                              LeftTime(ref Objs, floorCount, placementSystem.ySizeDis - 1, childCount);
                          if (roomCount + 1 != placementSystem.ySizeDis)
                              RightTime(ref Objs, floorCount, roomCount + 1, childCount);
                          else
                              RightTime(ref Objs, floorCount, 0, childCount);
                      }
                  }*/
            }
    }
    private void DownTime(ref List<GameObject> Objs, int floorCount, int roomCount, int childCount)
    {
        ItemData itemData = ItemData.Instance;
        PlacementSystem placementSystem = PlacementSystem.Instance;

        if (floorCount != -1)
            if (placementSystem.floorBool[floorCount, roomCount])
            {
                if (CheckObjs(ref Objs, placementSystem.apartment[floorCount, roomCount]))
                    if (placementSystem.apartment[floorCount, roomCount].GetComponent<ObjectID>().childCount == childCount)
                    {
                        Objs.Add(placementSystem.apartment[floorCount, roomCount]);

                        if (floorCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);

                        if (roomCount - 1 != -1)
                            LeftTime(ref Objs, floorCount, roomCount - 1, childCount);
                        else
                            LeftTime(ref Objs, floorCount, placementSystem.ySizeDis - 1, childCount);

                        if (roomCount + 1 != placementSystem.ySizeDis)
                            RightTime(ref Objs, floorCount, roomCount + 1, childCount);
                        else
                            RightTime(ref Objs, floorCount, 0, childCount);
                    }
                    else { }
                else
                {
                    print("HG");
                }
                /*     else
                   {
                       if (floorCount - 1 != -1)
                       {
                           DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                           if (roomCount - 1 != -1)
                               LeftTime(ref Objs, floorCount, roomCount - 1, childCount);
                           else
                               LeftTime(ref Objs, floorCount, placementSystem.ySizeDis - 1, childCount);
                           if (roomCount + 1 != placementSystem.ySizeDis)
                               RightTime(ref Objs, floorCount, roomCount + 1, childCount);
                           else
                               RightTime(ref Objs, floorCount, 0, childCount);
                       }
                   }
                 */
            }
    }
    private void LeftTime(ref List<GameObject> Objs, int floorCount, int roomCount, int childCount)
    {
        ItemData itemData = ItemData.Instance;
        PlacementSystem placementSystem = PlacementSystem.Instance;
        if (roomCount != -1)
            if (placementSystem.floorBool[floorCount, roomCount])
            {
                if (CheckObjs(ref Objs, placementSystem.apartment[floorCount, roomCount]))
                {
                    if (placementSystem.apartment[floorCount, roomCount].GetComponent<ObjectID>().childCount == childCount)
                    {
                        Objs.Add(placementSystem.apartment[floorCount, roomCount]);

                        if (roomCount - 1 != -1)
                            LeftTime(ref Objs, floorCount, roomCount - 1, childCount);
                        else
                            LeftTime(ref Objs, floorCount, placementSystem.ySizeDis - 1, childCount);

                        if (roomCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);

                        if (roomCount + 1 != placementSystem.xSizeDis)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                    }
                }
                else
                {
                    print("HG");
                }
                /*  else
                  {
                      if (roomCount - 1 != -1)
                      {
                          if (roomCount - 1 != -1)
                              DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                          if (roomCount + 1 != placementSystem.xSizeDis)
                              UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                          LeftTime(ref Objs, floorCount, roomCount - 1, childCount);
                      }
                      else
                      {
                          if (roomCount - 1 != -1)
                              DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                          if (roomCount + 1 != placementSystem.xSizeDis)
                              UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                          LeftTime(ref Objs, floorCount, placementSystem.ySizeDis - 1, childCount);
                      }
                  }
                */
            }
            else { }
        else
        {
            roomCount = placementSystem.ySizeDis - 1;
            if (placementSystem.floorBool[floorCount, roomCount])
                if (CheckObjs(ref Objs, placementSystem.apartment[floorCount, roomCount]))
                    if (placementSystem.apartment[floorCount, roomCount].GetComponent<ObjectID>().childCount == childCount)
                    {
                        Objs.Add(placementSystem.apartment[floorCount, roomCount]);

                        LeftTime(ref Objs, floorCount, roomCount - 1, childCount);

                        if (roomCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                        if (roomCount + 1 != placementSystem.xSizeDis)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                    }
                    else { }
                else
                {
                    print("HG");
                }
            /*   else
                {
                    if (roomCount - 1 != -1)
                        DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                    if (roomCount + 1 != placementSystem.xSizeDis)
                        UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                    LeftTime(ref Objs, floorCount, roomCount - 1, childCount);
                }*/
        }
    }
    private void RightTime(ref List<GameObject> Objs, int floorCount, int roomCount, int childCount)
    {
        ItemData itemData = ItemData.Instance;
        PlacementSystem placementSystem = PlacementSystem.Instance;

        if (roomCount != placementSystem.ySizeDis)
            if (placementSystem.floorBool[floorCount, roomCount])
            {
                if (CheckObjs(ref Objs, placementSystem.apartment[floorCount, roomCount]))
                    if (placementSystem.apartment[floorCount, roomCount].GetComponent<ObjectID>().childCount == childCount)
                    {
                        Objs.Add(placementSystem.apartment[floorCount, roomCount]);

                        if (roomCount + 1 != placementSystem.ySizeDis)
                            RightTime(ref Objs, floorCount, roomCount + 1, childCount);
                        else
                            RightTime(ref Objs, floorCount, 0, childCount);

                        if (roomCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);

                        if (roomCount + 1 != placementSystem.xSizeDis)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                    }
                    else { }
                else
                {
                    print("HG");
                }
                /*    else
                    {
                        if (roomCount + 1 != placementSystem.ySizeDis)
                        {
                            if (roomCount - 1 != -1)
                                DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                            if (roomCount + 1 != placementSystem.xSizeDis)
                                UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                            RightTime(ref Objs, floorCount, roomCount + 1, childCount);
                        }
                        else
                        {
                            if (roomCount - 1 != -1)
                                DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                            if (roomCount + 1 != placementSystem.xSizeDis)
                                UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                            RightTime(ref Objs, floorCount, roomCount + 1, childCount);
                        }
                    }*/
            }
            else { }
        else
        {
            roomCount = 0;
            if (placementSystem.floorBool[floorCount, roomCount])
                if (CheckObjs(ref Objs, placementSystem.apartment[floorCount, roomCount]))
                    if (placementSystem.apartment[floorCount, roomCount].GetComponent<ObjectID>().childCount == childCount)
                    {
                        Objs.Add(placementSystem.apartment[floorCount, roomCount]);

                        if (roomCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                        if (roomCount + 1 != placementSystem.xSizeDis)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);
                        RightTime(ref Objs, floorCount, roomCount + 1, childCount);
                    }
                    else { }
                else
                {
                    print("HG");
                }
        }
    }
    private bool CheckObjs(ref List<GameObject> Objs, GameObject obj)
    {
        foreach (GameObject item in Objs) if (item == obj) return false;
        return true;
    }


    /*  private void FirstMove()
      {
          ObjectManager objectManager = ObjectManager.Instance;

          objectManager.isFree = true;
          isFree = true;
          isDown = false;
          isSelected = true;
          objectManager.firstSpace = true;
          objectManager.firstObject = gameObject;
          objectManager.tempObjectCount = objectID.childCount;
          gameObject.transform.GetChild(objectID.childCount).gameObject.layer = 6;
          StartCoroutine(Move());
          objectManager.isFree = false;
      }
      private void SecondMove()
      {
          ObjectManager objectManager = ObjectManager.Instance;

          objectManager.isFree = true;
          isFree = true;
          isDown = false;
          isSelected = true;
          objectManager.secondSpace = true;
          objectManager.secondObject = gameObject;
          gameObject.transform.GetChild(objectID.childCount).gameObject.layer = 6;
          StartCoroutine(Move());
          objectManager.isFree = false;
      }
      private IEnumerator ThridMove()
      {
          ObjectManager objectManager = ObjectManager.Instance;

          objectManager.isFree = true;
          isFree = true;
          isDown = false;
          isSelected = true;
          objectManager.thridSpace = true;
          objectManager.thridObject = gameObject;
          gameObject.transform.GetChild(objectID.childCount).gameObject.layer = 6;
          StartCoroutine(Move());
          yield return new WaitForSeconds(0.3f);
          objectManager.isFree = false;
          objectManager.ObjectCorrect();
      }*/
    private IEnumerator Move()
    {
        ItemData itemData = ItemData.Instance;

        float lerpCount = 0;

        Vector3 finishPos = Vector3.zero;

        if (Mathf.Abs(transform.position.x) == Mathf.Abs(transform.position.z))
            finishPos = new Vector3(0.75f * (transform.position.x / Mathf.Abs(transform.position.x)), 0, 0.75f * (transform.position.z / Mathf.Abs(transform.position.z))) + transform.position;
        else if (Mathf.Abs(transform.position.x) > Mathf.Abs(transform.position.z))
            finishPos = new Vector3(0.75f * (transform.position.x / Mathf.Abs(transform.position.x)), 0, 0) + transform.position;
        else
            finishPos = new Vector3(0, 0, 0.75f * (transform.position.z / Mathf.Abs(transform.position.z))) + transform.position;


        while (isSelected)
        {
            lerpCount += Time.deltaTime * 5;
            transform.position = Vector3.Lerp(transform.position, finishPos, lerpCount);
            yield return new WaitForSeconds(Time.deltaTime);
            if (0.1f > Vector3.Distance(transform.position, finishPos))
                break;
        }
    }

}
