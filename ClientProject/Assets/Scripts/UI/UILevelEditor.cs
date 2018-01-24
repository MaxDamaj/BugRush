using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BugRush.UI {
    public class UILevelEditor : MonoBehaviour {

        [SerializeField]
        private Button _buttonSample;
        [SerializeField]
        private Button _moveCameraButton;
        [SerializeField]
        private RectTransform _container;

        void Start() {
            int segmentsCount = Resources.LoadAll<GameObject>("Segments").GetLength(0);
            for (int i=0; i<segmentsCount; i++) {
                string name = ((SegmentType)i).ToString();
                GameObject tmp = Instantiate(_buttonSample.gameObject, _container);
                SegmentType type = (SegmentType)i;
                tmp.name = name+"Button";
                tmp.GetComponentInChildren<Text>().text = name;

                tmp.GetComponent<Button>().onClick.AddListener(delegate {
                    ResetButtons();
                    tmp.GetComponent<Button>().interactable = false;
                    GlobalData.Instance.newSegmentType = type;
                    GlobalData.Instance.isEnableEdit = true;
                    });
            }
            _moveCameraButton.onClick.AddListener(EnableMoveCamera);
            _buttonSample.gameObject.SetActive(false);
        }

        void EnableMoveCamera() {
            GlobalData.Instance.isEnableEdit = false;
            ResetButtons();
            _moveCameraButton.interactable = false;
        }

        void ResetButtons() {
            for (int i=0; i<_container.childCount; i++) {
                _container.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }
    }
}
