﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diviner.CameraController
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("Camera Zoom Settings")]
        public bool CameraCanZoom = true;

        [Space]
        public Vector3 CameraDirection;

        [Header("Camera Movement Settings")]
        [Range(0, 1)]
        public float CameraSpeed;

        [Header("Camera Direction")]
        public bool LookAtTarget = false;

        [Header("Camera Offset Settings")]
        public Vector3 PositionOffset;

        [Header("Camera Target Settings")]
        public Transform Target;

        [Space]
        public Vector2 ZoomDistanceMinMax = new Vector2(5, 30);

        [Space]
        [Range(0, 1)]
        public float ZoomSpeed;

        private Transform CameraObject => transform.GetChild(0);
        private Camera MainCamera => CameraObject.GetComponent<Camera>();

        private void LateUpdate()
        {
            if (Target == null)
            {
                Debug.LogError("Camera has no target");

                return;
            }

            if (CameraCanZoom)
            {
                if (MainCamera.orthographic)
                {
                    UpdateOrthographicZoom();
                }
                else
                {
                    UpdatePerspectiveZoom();
                }
            }

            UpdateCameraPosition();

            UpdateCameraDirection();
        }

        private void Start()
        {
            UpdateCameraOffset();
        }

        #region Zoom

        //TODO: Fix duplication and refactor methods

        private void UpdatePerspectiveZoom()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            float distance = Vector3.Distance(Target.position, CameraObject.position);

            if (distance <= ZoomDistanceMinMax.x && scrollInput > 0)
            {
                return;
            }
            else if (distance >= ZoomDistanceMinMax.y && scrollInput < 0)
            {
                return;
            }

            CameraObject.position += CameraObject.forward * scrollInput * ZoomSpeed;
        }

        private void UpdateOrthographicZoom()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (MainCamera.orthographicSize <= ZoomDistanceMinMax.x && scrollInput > 0)
            {
                return;
            }
            else if (MainCamera.orthographicSize >= ZoomDistanceMinMax.y && scrollInput < 0)
            {
                return;
            }

            MainCamera.orthographicSize += scrollInput * ZoomSpeed;
        }

        #endregion Zoom

        #region Follow Player

        private void UpdateCameraOffset()
        {
            CameraObject.position = transform.position + PositionOffset;
        }

        private void UpdateCameraPosition()
        {
            transform.position = Vector3.Lerp(transform.position, Target.position, CameraSpeed);
        }

        #endregion Follow Player

        #region Camera Direction

        private void UpdateCameraDirection()
        {
            if (LookAtTarget)
            {
                CameraObject.LookAt(Target);
            }
            else
            {
                CameraObject.rotation = Quaternion.Euler(CameraDirection.x, CameraDirection.y, CameraDirection.z);
            }
        }

        #endregion Camera Direction
    }
}
