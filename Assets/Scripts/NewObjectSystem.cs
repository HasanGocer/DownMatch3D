using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectSystem : MonoSingleton<NewObjectSystem>
{
    [SerializeField] int _maxMisObjectCount;

    public void CheckFloor()
    {
        ItemData itemData = ItemData.Instance;
        PlacementSystem placementSystem = PlacementSystem.Instance;

        int floorCount;

        for (int i1 = 0; i1 < placementSystem.ySizeDis; i1++)
        {
            floorCount = 0;

            CheckRoomCount(ref floorCount, i1, itemData.field.floorCount - 1, placementSystem);

            if (floorCount != 0)
                for (int i2 = 0; i2 < floorCount; i2++)
                    GetNewObject(itemData.field.floorCount - 1 - i2, i1, placementSystem);
        }
    }

    private void CheckRoomCount(ref int floorCount, int roomCount, int maxFloorCount, PlacementSystem placementSystem)
    {
        while (!placementSystem.floorBool[maxFloorCount - floorCount, roomCount])
        {
            floorCount++;
            if (maxFloorCount - floorCount == -1) break;
        }
    }
    private void GetNewObject(int floorCount, int roomCount, PlacementSystem placementSystem)
    {
        int childCount = Random.Range(0, 4);
        childCount += ItemData.Instance.field.objectCount;
        childCount = childCount % placementSystem.objectCount;
        GameObject obj = placementSystem.AddObject(floorCount, roomCount, childCount);
        obj.transform.position += new Vector3(0, 4, 0);
        StartCoroutine(ObjectManager.Instance.Move(obj, placementSystem.apartmentPos[floorCount, roomCount]));

    }
}
