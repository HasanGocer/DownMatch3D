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

        if (GameManager.Instance.gameStat == GameManager.GameStat.start && objectManager.isFree && !isFree && CounterSystem.Instance.GetCounterCount() > 0)
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
                CoinSystem.Instance.CoinStart();
            objectManager.OffTime(objs);

            isFree = false;
            FinishSystem.Instance.CheckFail();
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
            }
    }
    private void DownTime(ref List<GameObject> Objs, int floorCount, int roomCount, int childCount)
    {
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

                        if (floorCount + 1 != itemData.field.floorCount)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);

                        if (floorCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                    }
                }
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

                        if (roomCount + 1 != itemData.field.floorCount)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);

                        if (roomCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                    }
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

                        if (floorCount + 1 != itemData.field.floorCount)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);

                        if (floorCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                    }
                    else { }
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

                        RightTime(ref Objs, floorCount, roomCount + 1, childCount);

                        if (floorCount + 1 != itemData.field.floorCount)
                            UpTime(ref Objs, floorCount + 1, roomCount, childCount);

                        if (floorCount - 1 != -1)
                            DownTime(ref Objs, floorCount - 1, roomCount, childCount);
                    }
        }
    }
    private bool CheckObjs(ref List<GameObject> Objs, GameObject obj)
    {
        foreach (GameObject item in Objs) if (item == obj) return false;
        return true;
    }
}
