using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public enum AxisOption
        {
            // Options for which axes to use
            Both, // Use both
            OnlyHorizontal, // Only horizontal
            OnlyVertical // Only vertical
        }

        public bool isZoom;
        public int MovementRange = 100;
        public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
        public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
        public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

        Vector3 m_StartPos;
        bool m_UseX; // Toggle for using the x axis
        bool m_UseY; // Toggle for using the Y axis
        CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input
        Transform _transform;
        int delta;
        Vector3 newPos;
        public static Vector3 deltaVec, deltaVecZoom;

        void Start()
        {
            m_StartPos = transform.position;
            CreateVirtualAxes();
            _transform = transform;
        }

        void UpdateVirtualAxes(Vector3 value)
        {
            if (isZoom)
            {
                deltaVecZoom = m_StartPos - value;
                deltaVecZoom.y = -deltaVecZoom.y;
                deltaVecZoom /= MovementRange;

                m_VerticalVirtualAxis.Update(deltaVecZoom.y);
            }
            else
            {
                deltaVec = m_StartPos - value;
                deltaVec.y = -deltaVec.y;
                deltaVec /= MovementRange;
                if (m_UseX)
                {
                    m_HorizontalVirtualAxis.Update(-deltaVec.x);

                }

                if (m_UseY)
                {
                    m_VerticalVirtualAxis.Update(deltaVec.y);
                }
            }
            
        }

        void CreateVirtualAxes()
        {
            // set axes to use
            m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
            m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

            // create new axes based on axes to use
            if (m_UseX)
            {
                m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
            }
            if (m_UseY)
            {
                m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
            }
        }


        public void OnDrag(PointerEventData data)
        {
            newPos = Vector3.zero;

            if (m_UseX)
            {
                delta = (int)(data.position.x - m_StartPos.x);
                newPos.x = delta;
            }

            if (m_UseY)
            {
                delta = (int)(data.position.y - m_StartPos.y);
                newPos.y = delta;
            }
            _transform.position = Vector3.ClampMagnitude(newPos, MovementRange) + m_StartPos;
            UpdateVirtualAxes(transform.position);
        }


        public void OnPointerUp(PointerEventData data)
        {
            _transform.position = m_StartPos;
            UpdateVirtualAxes(m_StartPos);
        }


        public void OnPointerDown(PointerEventData data) { }

        void OnDisable()
        {
            // remove the joysticks from the cross platform input
            if (m_UseX)
            {
                m_HorizontalVirtualAxis.Remove();
            }
            if (m_UseY)
            {
                m_VerticalVirtualAxis.Remove();
            }
        }
    }
}