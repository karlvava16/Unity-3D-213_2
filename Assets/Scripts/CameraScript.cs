﻿using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform fpvTransform;

    private InputAction lookAction;
    private Vector3 c;
    private bool fpv = true;
    private float mX, mY;
    private float sensitivityH = 10.0f, sensitivityV = 5.0f, sensitivityW = 0.1f;
    private float fpvRange = 0.6f;
    private float maxDistance = 5f; // Максимальна відстань

    void Start()
    {
        c = this.transform.position - player.transform.position;
        mX = this.transform.eulerAngles.y;
        mY = this.transform.eulerAngles.x;
        lookAction = InputSystem.actions.FindAction("Look");
    }

    private void Update()
    {
        if (fpv)
        {
            Vector2 mouseWheel = Input.mouseScrollDelta;
            if (mouseWheel.y != 0)
            {
                if (c.magnitude > maxDistance)
                {
                    c = c.normalized * maxDistance;
                }
                if (c.magnitude > fpvRange)
                {
                    c = c * (1 - mouseWheel.y * sensitivityW);
                    if (c.magnitude < fpvRange)
                    {
                        c = c * 0.01f;
                        GameState.isFpv = true;
                    }

                }
                else
                {
                    if (mouseWheel.y < 0)
                    {
                        c = c / c.magnitude * fpvRange * 1.01f;
                        GameState.isFpv = false;
                    }

                }
            }

            Vector2 lookValue = lookAction.ReadValue<Vector2>() * Time.deltaTime;
            mX += lookValue.x * sensitivityH;        // Input.GetAxis("Mouse X");
            float my = -lookValue.y * sensitivityV;  // -Input.GetAxis("Mouse Y");
            if (0 <= mY + my && mY + my <= 75)
            {
                mY += my;
            }
            this.transform.eulerAngles = new Vector3(mY, mX, 0);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            fpv = !fpv;
            if (!fpv)
            {
                this.transform.position = fpvTransform.position;
                this.transform.rotation = fpvTransform.rotation;
            }
        }
    }

    void LateUpdate()
    {
        if (fpv)
        {
            this.transform.position = Quaternion.Euler(0, mX, 0) * c +
                player.transform.position;
        }
    }
}
