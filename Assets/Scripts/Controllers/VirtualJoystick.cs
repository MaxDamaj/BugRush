using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace BugRush.Controllers {
    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

        [SerializeField]
        private Image _stickBG = null;
        [SerializeField]
        private Image _stickHandle = null;

        private Vector2 _inputVector;
        private static VirtualJoystick _instance;

        #region API

        public static VirtualJoystick Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<VirtualJoystick>();
                }
                return _instance;
            }
        }

        void Start() {

        }

        #endregion

        #region Events

        public void OnDrag(PointerEventData eventData) {
            Vector2 position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_stickBG.rectTransform, eventData.position, eventData.pressEventCamera, out position)) {
                position.x = (position.x / _stickBG.rectTransform.sizeDelta.x);
                position.y = (position.y / _stickBG.rectTransform.sizeDelta.y);
                _inputVector = new Vector3(position.x * 2 + 1, position.y * 2 - 1);
                _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

                _stickHandle.rectTransform.anchoredPosition =
                    new Vector2(_inputVector.x * (_stickHandle.rectTransform.sizeDelta.x / 2),
                                _inputVector.y * (_stickHandle.rectTransform.sizeDelta.y / 2));
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData) {
            _inputVector = Vector2.zero;
            _stickHandle.rectTransform.anchoredPosition = Vector2.zero;
        }

        #endregion

        public float Horizontal() {
            return _inputVector.x;
        }
        public float Vertical() {
            return _inputVector.y;
        }
    }
}
