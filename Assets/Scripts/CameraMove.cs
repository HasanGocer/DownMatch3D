using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoSingleton<CameraMove>
{
    Touch touch;
    Vector2 _firstPos;
    [SerializeField] Transform _target;
    Vector2 _targetPos;
    public Vector2 maxTargetPos;
    [SerializeField] float _CamDistance;
    public float yDistance;
    public GameObject focusObject;
    public bool isObjectTouch, isMove;
    public void Start()
    {
        if (GameManager.Instance.level % 2 == 0)
            transform.position = new Vector3(0, ItemData.Instance.field.floorCount / 2, -5 * yDistance);
        else
            transform.position = new Vector3(0, ItemData.Instance.field.floorCount / 2, -7 * yDistance);
    }


    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _firstPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    /*   if (30 < Mathf.Abs(_firstPos.y - touch.position.y))
                       {
                           _targetPos.y -= (_firstPos.y - touch.position.y) / (Camera.main.pixelHeight / 10);
                           _targetPos.y = Mathf.Clamp(_targetPos.y, -maxTargetPos.y, 0);
                           _target.position = new Vector3(_target.position.x, _targetPos.y, _target.position.z);
                           isMove = true;
                       }*/

                    if (10 < Mathf.Abs(_firstPos.x - touch.position.x))
                    {
                        _targetPos.x -= (touch.position.x - _firstPos.x) / (Camera.main.pixelWidth / 250);
                        _target.transform.rotation = Quaternion.Euler(new Vector3(0, _targetPos.x, 0));
                        isMove = true;
                    }

                    _firstPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    _firstPos = Vector2.zero;
                    if (isObjectTouch && !isMove && focusObject != null) focusObject.GetComponent<ObjectTouch>().Touch();
                    isMove = false;
                    focusObject = null;
                    break;
            }
        }
    }
}
