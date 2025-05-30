using System;
using UnityEngine;

namespace _Scripts
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 _inputVector;
        private Quaternion _inputQuaternion;
        private float _screenWidthCenter;
        private bool _isJumping;

        private Vector2 _startSwipePosition;
        private Vector2 _endSwipePosition;

        public Vector3 InputVector => _inputVector;
        public Quaternion InputQuaternion => _inputQuaternion;
        public bool IsJumping => _isJumping;

        private float _xInput;
        private float _rotateAngle;

        public float XInput
        {
            get => _xInput;
            set
            {
                if (value is >= -1 and <= 1)
                {
                    _xInput = value;
                }
            }
        }

        public float RotateAngle
        {
            get => _rotateAngle;
            set
            {
                if (value is >= -20 and <= 20)
                {
                    _rotateAngle = value;
                }
            }
        }

        private void Awake()
        {
            _screenWidthCenter = (float)Screen.width / 2;
        }

        private void InputHandler()
        {

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.position.x < _screenWidthCenter)
                {
                    XInput--;
                    RotateAngle++;
                }

                if (touch.position.x > _screenWidthCenter)
                {
                    XInput++;
                    RotateAngle--;
                }

                if (touch.phase == TouchPhase.Began)
                {
                    _startSwipePosition = touch.position;
                }

                if (touch.position.y - _startSwipePosition.y > 50f)
                {
                    _isJumping = true;
                }

            }
            else
            {
                XInput = 0;
                RotateAngle = 0;
                _isJumping = false;
            }

            _inputVector = new Vector3(XInput, 0, 0);
            _inputQuaternion = Quaternion.AngleAxis(RotateAngle, Vector3.down);
        }

        private void Update()
        {
            InputHandler();
        }
        
        public void ResetInput()
        {
            _isJumping = false;
            _inputVector = Vector3.zero;
            _inputQuaternion = Quaternion.identity;
            _xInput = 0;
            _rotateAngle = 0;
        }
    }
}
