using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoSingleton<PlacementSystem>
{
    [SerializeField] int _OPSortObject;

    public int objectCount;
    [HideInInspector] public int xSizeDis, ySizeDis;
    [SerializeField] GameObject _main, _mainCylinder;

    public bool[,] floorBool;
    public GameObject[,] apartment, apartmentPos;

    public void FinishPartical()
    {
        ParticalManager.Instance.CalLFinishPartical(_main);
    }
    public void finishTime()
    {
        StartCoroutine(FinishMove(_main, _main.transform.position));
    }

    public void ObjectBack(GameObject obj)
    {
        ObjectPool.Instance.AddObject(_OPSortObject, obj);
    }
    public GameObject AddObject(int floorCount, int roomCount, int childCount)
    {
        GameObject pos, obj;

        floorBool[floorCount, roomCount] = true;
        pos = apartmentPos[floorCount, roomCount];
        obj = ObjectPool.Instance.GetPooledObject(_OPSortObject, pos.transform.position);
        obj.transform.rotation = apartmentPos[floorCount, roomCount].transform.rotation;
        obj.transform.SetParent(null);
        apartment[floorCount, roomCount] = null;
        apartment[floorCount, roomCount] = obj;
        obj.transform.SetParent(_main.transform);
        ObjectID objectID = obj.GetComponent<ObjectID>();
        objectID.childs[childCount].SetActive(true);

        objectID.floorCount = floorCount;
        objectID.roomCount = roomCount;
        objectID.childCount = childCount;

        return obj;
    }

    public void StartPlacement()
    {
        CylinderPlacement();
    }
    public void ObjectPlacement()
    {
        CylinderObjectPlacement();
    }

    private void CylinderPlacement()
    {
        CylinderArrayReSize();
        CylinderPosPlacement();
    }
    private void CylinderArrayReSize()
    {
        ItemData itemData = ItemData.Instance;

        xSizeDis = itemData.field.floorCount;
        ySizeDis = 360 / 15;

        floorBool = new bool[itemData.field.floorCount, 360 / 15];
        apartment = new GameObject[itemData.field.floorCount, 360 / 15];
        apartmentPos = new GameObject[itemData.field.floorCount, 360 / 15];
    }
    private void CylinderPosPlacement()
    {
        ItemData itemData = ItemData.Instance;
        _mainCylinder.SetActive(true);
        _mainCylinder.transform.localScale = new Vector3(itemData.field.sizeCount * 3f, itemData.field.floorCount * 2, itemData.field.sizeCount * 3);

        for (int i1 = 0; i1 < itemData.field.floorCount; i1++)
        {
            _mainCylinder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            for (int i2 = 0; i2 < 360 / 15; i2++)
            {
                GameObject pos = new GameObject("pos");

                pos.transform.position = _mainCylinder.transform.TransformDirection(Vector3.forward * ((float)itemData.field.sizeCount + 1));
                pos.transform.position += new Vector3(0, i1, 0);
                pos.transform.LookAt(_mainCylinder.transform);
                pos.transform.rotation = Quaternion.Euler(new Vector3(0, pos.transform.rotation.eulerAngles.y, 0));
                pos.transform.rotation *= Quaternion.Euler(new Vector3(0, 90, 0));

                _mainCylinder.transform.rotation *= Quaternion.Euler(new Vector3(0, i2 * 15, 0));

                pos.transform.SetParent(_mainCylinder.transform);

                floorBool[i1, i2] = false;
                apartmentPos[i1, i2] = pos;
            }
        }

    }
    private void CylinderObjectPlacement()
    {
        ItemData itemData = ItemData.Instance;
        int childCount;

        for (int i1 = 0; i1 < itemData.field.floorCount; i1++)
            for (int i2 = 0; i2 < 360 / 15; i2++)
            {
                childCount = Random.Range(0, 4);
                childCount += ItemData.Instance.field.objectCount;
                childCount = childCount % objectCount;

                AddObject(i1, i2, childCount);
            }

    }

    private IEnumerator FinishMove(GameObject moveObj, Vector3 finishPos)
    {
        Buttons buttons = Buttons.Instance;
        ObjectTouch objectTouch = moveObj.GetComponent<ObjectTouch>();
        float lerpCount = 0;
        Vector3 firstPos = moveObj.transform.position;

        while (lerpCount < 1)
        {
            lerpCount += Time.deltaTime * 2;
            moveObj.transform.position = Vector3.Lerp(firstPos, finishPos + new Vector3(0, 0.3f, 0), lerpCount);
            yield return null;
        }
        lerpCount = 0;
        firstPos = moveObj.transform.position;
        while (lerpCount < 1)
        {
            lerpCount += Time.deltaTime * 3;
            moveObj.transform.position = Vector3.Lerp(firstPos, finishPos + new Vector3(0, -20, 0), lerpCount);
            yield return null;
        }

        _main.SetActive(false);
        FinishPartical();

        LevelManager.Instance.LevelCheck();

        buttons.winPanel.SetActive(true);
    }
}
