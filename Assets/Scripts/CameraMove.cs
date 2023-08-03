using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoSingleton<CameraMove>
{
    Touch touch;
    Vector2 _firstPos;
    Vector2 _targetPos;

    [SerializeField] Transform _target;

    [SerializeField] float _yDistance;
    [SerializeField] float _distanceFactor;

    [HideInInspector] public GameObject focusObject;
    public bool isObjectTouch, isMove;

    public void Start()
    {
        transform.position = new Vector3(0, ItemData.Instance.field.floorCount / 2, _distanceFactor * _yDistance);
    }


    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    BeganTime();
                    break;

                case TouchPhase.Moved:
                    MovedTime();
                    break;

                case TouchPhase.Ended:
                    EndedTime();
                    break;
            }
        }
    }

    private void BeganTime()
    {
        _firstPos = touch.position;
    }
    private void MovedTime()
    {
        if (10 < touch.deltaPosition.x)
        {
            _targetPos.x -= (touch.position.x - _firstPos.x) / (Camera.main.pixelWidth / 250);
            _target.transform.rotation = Quaternion.Euler(new Vector3(0, _targetPos.x, 0));
            isMove = true;
        }

        _firstPos = touch.position;
    }
    private void EndedTime()
    {
        _firstPos = Vector2.zero;
        if (isObjectTouch && !isMove && focusObject != null) focusObject.GetComponent<ObjectTouch>().Touch();
        isMove = false;
        focusObject = null;
    }
}
