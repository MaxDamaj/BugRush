using BugRush.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BugRush.UI {
    public class UIResultWindow : MonoBehaviour {

        [Serializable]
        public struct ResultMessage {
            public PlayState state;
            public string text;
            public Color color;
        }

        [SerializeField]
        private RectTransform _window = null;
        [SerializeField]
        private Text _messageText = null;
        [SerializeField]
        private Image[] _coloredImage = null;

        public List<ResultMessage> messages;

        void Start() {
            _window.gameObject.SetActive(false);
            GameController.OnPlayStateChange += RefreshUI;
        }

        void RefreshUI(PlayState state) {
            ResultMessage msg = messages.Find(x => x.state == state);
            ShowMessage(msg.text, msg.color);
        }

        void ShowMessage(string text, Color color) {
            _window.gameObject.SetActive(true);
            _messageText.text = text;
            foreach (var image in _coloredImage) {
                image.color = color;
            }
        }

        void OnDestroy() {
            GameController.OnPlayStateChange -= RefreshUI;
        }
    }
}
