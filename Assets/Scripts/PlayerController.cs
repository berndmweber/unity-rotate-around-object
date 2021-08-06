/*
 * SPDX-FileCopyrightText: Copyright (c) 2021 Bernd Weber
 * SPDX-License-Identifier: BSD-3-Clause
 *
 * Copyright (c) 2021, Bernd Weber
 * All rights reserved.
 */

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform MainCamera;
    [SerializeField] float MaxRotationSpeed = 10.0f;
    [SerializeField] float RotationAcceleration = 2.5f;
    [SerializeField] float DefaultAltitude = 1.5f;
    [SerializeField] float MinAltitude = 1.1f;
    [SerializeField] float MaxAltitude = 3.0f;
    [SerializeField] float ZoomSpeed = 0.1f;

    private Vector2 ActualRotationSpeed = Vector2.zero;
    private Vector2 MinLerpRotationSpeed = Vector2.zero;
    private Vector2 MaxLerpRotationSpeed = Vector2.zero;
    private Vector2 RotationDirection = Vector2.zero;
    private Vector2 RotationLerpV = Vector2.zero;
    private bool DirectionChangeX = false;
    private bool DirectionChangeY = false;


    // Start is called before the first frame update
    void Start()
    {
        if (MainCamera)
        {
            MainCamera.LookAt(transform);
            
            // Ensure we are starting at the default altitude
            float distance = Vector3.Distance(MainCamera.position, transform.position);
            if (distance != DefaultAltitude)
            {
                // The forward vector corresponds to a vector of length 1
                MainCamera.position += MainCamera.forward * (distance - DefaultAltitude);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MainCamera)
        {
            CheckMovementKeys();

            RotateCamera();

            ZoomCamera();

            // Reset Direction change flags
            DirectionChangeX = false;
            DirectionChangeY = false;
        }
    }

    private void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y > 0.0f)
        {
            if (Vector3.Distance(MainCamera.position, transform.position) >= MinAltitude)
            {
                // Zoom in
                MainCamera.position += MainCamera.forward * ZoomSpeed;
            }
        }
        else if (Input.mouseScrollDelta.y < 0.0f)
        {
            if (Vector3.Distance(MainCamera.position, transform.position) <= MaxAltitude)
            {
                // Zoom out
                MainCamera.position -= MainCamera.forward * ZoomSpeed;
            }
        }
    }

    private void CheckMovementKeys()
    {
        if ((Input.GetKeyUp(KeyCode.D) && !Input.GetKeyDown(KeyCode.A)) ||
            (Input.GetKeyUp(KeyCode.A) && !Input.GetKeyDown(KeyCode.D)))
        {
            DirectionChangeX = true;
            RotationDirection.x = 0f;
        }
        if ((Input.GetKeyUp(KeyCode.W) && !Input.GetKeyDown(KeyCode.S)) ||
            (Input.GetKeyUp(KeyCode.S) && !Input.GetKeyDown(KeyCode.W)))
        {
            DirectionChangeY = true;
            RotationDirection.y = 0f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Mathf.Approximately(0.0f, RotationDirection.x) || Mathf.Approximately(-1.0f, RotationDirection.x))
            {
                DirectionChangeX = true;
                RotationDirection.x = 1.0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Mathf.Approximately(0.0f, RotationDirection.x) || Mathf.Approximately(1.0f, RotationDirection.x))
            {
                DirectionChangeX = true;
                RotationDirection.x = -1.0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Mathf.Approximately(0.0f, RotationDirection.y) || Mathf.Approximately(-1.0f, RotationDirection.y))
            {
                DirectionChangeY = true;
                RotationDirection.y = 1.0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Mathf.Approximately(0.0f, RotationDirection.y) || Mathf.Approximately(1.0f, RotationDirection.y))
            {
                DirectionChangeY = true;
                RotationDirection.y = -1.0f;
            }
        }
    }

    private void RotateCamera()
    {
        // Only do this if we rotate to a direction or if we still have rotation speed
        if (!Mathf.Approximately(0.0f, RotationDirection.x) || !Mathf.Approximately(0.0f, ActualRotationSpeed.x))
        {
            // Resetting values whenever we change direction
            if (DirectionChangeX)
            {
                MinLerpRotationSpeed.x = ActualRotationSpeed.x;
                if (Mathf.Approximately(RotationDirection.x, 0f))
                {
                    MaxLerpRotationSpeed.x = 0.0f;
                } else if (RotationDirection.x > 0f)
                {
                    MaxLerpRotationSpeed.x = MaxRotationSpeed;
                } else
                {
                    MaxLerpRotationSpeed.x = -MaxRotationSpeed;
                }
                RotationLerpV.x = 0.0f;
            }

            // Only do this if we initiate movement or fade out of movement and only until we reach max values
            if ((!Mathf.Approximately(RotationDirection.x, 0f) && !Mathf.Approximately(MaxRotationSpeed, ActualRotationSpeed.x)) ||
                ( Mathf.Approximately(RotationDirection.x, 0f) && !Mathf.Approximately(0f, ActualRotationSpeed.x)))
            {
                ActualRotationSpeed.x = Mathf.Lerp(MinLerpRotationSpeed.x, MaxLerpRotationSpeed.x, RotationLerpV.x);
                RotationLerpV.x += RotationAcceleration * Time.deltaTime;
            }

            // Rotate left/right
            MainCamera.RotateAround(transform.position, MainCamera.up, -ActualRotationSpeed.x * Time.deltaTime);
        }
        if (!Mathf.Approximately(0.0f, RotationDirection.y) || !Mathf.Approximately(0.0f, ActualRotationSpeed.y))
        {
            if (DirectionChangeY)
            {
                MinLerpRotationSpeed.y = ActualRotationSpeed.y;
                if (Mathf.Approximately(RotationDirection.y, 0f))
                {
                    MaxLerpRotationSpeed.y = 0.0f;
                }
                else if (RotationDirection.y > 0f)
                {
                    MaxLerpRotationSpeed.y = MaxRotationSpeed;
                }
                else
                {
                    MaxLerpRotationSpeed.y = -MaxRotationSpeed;
                }
                RotationLerpV.y = 0.0f;
            }

            if ((!Mathf.Approximately(RotationDirection.y, 0f) && !Mathf.Approximately(MaxRotationSpeed, ActualRotationSpeed.y)) ||
                (Mathf.Approximately(RotationDirection.y, 0f) && !Mathf.Approximately(0f, ActualRotationSpeed.y)))
            {
                ActualRotationSpeed.y = Mathf.Lerp(MinLerpRotationSpeed.y, MaxLerpRotationSpeed.y, RotationLerpV.y);
                RotationLerpV.y += RotationAcceleration * Time.deltaTime;
            }

            // Rotate up/down
            MainCamera.RotateAround(transform.position, MainCamera.right, ActualRotationSpeed.y * Time.deltaTime);
        }
    }
}
