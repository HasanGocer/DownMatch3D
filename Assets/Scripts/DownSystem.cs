using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DownSystem : MonoSingleton<DownSystem>
{
    public void AllDown(ObjectID objectID)
    {
        DownTime(objectID.floorCount, objectID.roomCount);
    }
    void DownTime(int floorCount, int roomCount)
    {
        PlacementSystem placementSystem = PlacementSystem.Instance;
        bool isFinish = false;

        if (!placementSystem.floorBool[floorCount, roomCount])
            for (int i = floorCount + 1; i < ItemData.Instance.field.floorCount; i++)
                if (placementSystem.floorBool[i, roomCount])
                {
                    ChangeRoom(floorCount, roomCount, i, roomCount, ref isFinish);
                    if (i != ItemData.Instance.field.floorCount - 1)
                        DownTime(i, roomCount);
                    break;
                }

        if (!isFinish)
            ObjectManager.Instance.isFree = false;
    }

    private void ChangeRoom(int floorCount, int roomCount, int finishFloorCount, int finishRoomCount, ref bool isFinish)
    {
        PlacementSystem placementSystem = PlacementSystem.Instance;
        ObjectID objectID = placementSystem.apartment[finishFloorCount, finishRoomCount].GetComponent<ObjectID>();

        isFinish = true;

        placementSystem.apartment[floorCount, roomCount] = placementSystem.apartment[finishFloorCount, finishRoomCount];
        placementSystem.apartment[finishFloorCount, finishRoomCount] = null;
        placementSystem.floorBool[finishFloorCount, finishRoomCount] = false;
        placementSystem.floorBool[floorCount, roomCount] = true;

        objectID.floorCount = floorCount;
        objectID.roomCount = roomCount;

        Move(floorCount, roomCount, objectID);
    }
    private void Move(int finishFloorCount, int finishRoomCount, ObjectID objectID)
    {
        PlacementSystem placementSystem = PlacementSystem.Instance;
        GameObject obj = placementSystem.apartment[finishFloorCount, finishRoomCount];
        ObjectTouch objectTouch = obj.GetComponent<ObjectTouch>();

        if (!objectTouch.isDown)
            StartCoroutine(Move(obj, objectTouch, objectID));
    }
    private IEnumerator Move(GameObject moveObj, ObjectTouch objectTouch, ObjectID objectID)
    {
        PlacementSystem placementSystem = PlacementSystem.Instance;

        float lerpCount = 0;
        objectTouch.isDown = true;
        Vector3 firstPos = moveObj.transform.position;

        while (objectTouch.isDown && lerpCount < 1)
        {
            lerpCount += Time.deltaTime * 6;
            moveObj.transform.position = Vector3.Lerp(firstPos, placementSystem.apartmentPos[objectID.floorCount, objectID.roomCount].transform.position, lerpCount);
            yield return null;
        }
        objectTouch.isDown = false;
    }
}
