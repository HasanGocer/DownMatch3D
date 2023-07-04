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

        int roomCount, childCount;

        for (int i1 = 0; i1 < placementSystem.ySizeDis; i1++)
        {
            roomCount = 0;
            while (!placementSystem.floorBool[itemData.field.floorCount - 1 - roomCount, i1])
            {
                roomCount++;
                if (itemData.field.floorCount - 1 - roomCount == -1) break;
            }

            if (roomCount != 0)
                for (int i2 = 0; i2 < roomCount; i2++)
                {
                    childCount = Random.Range(0, 4);
                    childCount += ItemData.Instance.field.objectCount;
                    childCount = childCount % placementSystem.objectCount;
                    GameObject obj = placementSystem.AddObject(itemData.field.floorCount - 1 - i2, i1, childCount);
                    obj.transform.position += new Vector3(0, 4, 0);
                    StartCoroutine(ObjectManager.Instance.Move(obj, placementSystem.apartmentPos[itemData.field.floorCount - 1 - i2, i1]));
                }
        }
    }
}
