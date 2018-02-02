using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;

namespace BugRush.Controllers {
    public class UILevelEditor : MonoBehaviour, IPointerDownHandler {

        [Header("Side Buttons")]
        [SerializeField]
        private Button _buttonSample = null;
        [SerializeField]
        private Button _moveCameraButton = null;
        [SerializeField]
        private RectTransform _container = null;

        [Header("Upper Panel")]
        [SerializeField]
        private InputField _inputLevelTitle = null;
        [SerializeField]
        private Button _loadSampleButton = null;
        [SerializeField]
        private Button _saveButton = null;
        [SerializeField]
        private Button _loadButton = null;
        [SerializeField]
        private Text _messageText = null;

        [Header("Grid")]
        [SerializeField]
        private RectTransform _gridContainer = null;
        [SerializeField]
        private Image _segmentSample = null;

        private Sprite _newCellSprite;
        private Sprite _defaultCellSprite;
        private List<Image> _gridCells = null;
        private Dictionary<string, Sprite> _dictIcons;
        private Level level;

        #region API

        void Start() {
            level = new Level(50, 50, "emptyLevel");

            _gridCells = new List<Image>();
            _dictIcons = new Dictionary<string, Sprite>();
            _dictIcons = GameController.Instance.LoadResources<Sprite>("Icons/");
            _dictIcons.TryGetValue("icon" + SegmentType.Default.ToString(), out _defaultCellSprite);

            //Paste segments
            for (int i = 0; i < level.cells.Count; i++) {
                GameObject tmp = Instantiate(_segmentSample.gameObject, _gridContainer);
                tmp.SetActive(true);
                tmp.transform.localScale = Vector3.one;
                tmp.name = SegmentType.Default.ToString();
                Sprite sprite; _dictIcons.TryGetValue("icon" + SegmentType.Default.ToString(), out sprite);
                _gridCells.Add(tmp.GetComponent<Image>());
                _gridCells[i].sprite = sprite;
            }
            _segmentSample.gameObject.SetActive(false);

            int segmentsCount = GameController.Instance.dictSegments.Count;
            for (int i = 0; i < segmentsCount; i++) {
                string name = ((SegmentType)i).ToString();
                Sprite sprite; _dictIcons.TryGetValue("icon" + name, out sprite);
                GameObject tmp = Instantiate(_buttonSample.gameObject, _container);
                SegmentType type = (SegmentType)i;
                tmp.name = name + "Button";
                tmp.GetComponent<Image>().sprite = sprite;

                tmp.GetComponent<Button>().onClick.AddListener(delegate {
                    ResetButtons();
                    tmp.GetComponent<Button>().interactable = false;
                    _dictIcons.TryGetValue("icon" + type.ToString(), out _newCellSprite);
                    GlobalData.Instance.isEnableEdit = true;
                });
            }
            _buttonSample.gameObject.SetActive(false);

            //Buttons
            _messageText.text = "";
            _moveCameraButton.onClick.AddListener(EnableMoveCamera);
            _loadSampleButton.onClick.AddListener(delegate { DrawLevel(JsonUtility.FromJson<Level>(Resources.Load<TextAsset>("sampleLevel").text)); });
            _loadButton.onClick.AddListener(LoadLevel);
            _saveButton.onClick.AddListener(SaveLevel);
        }

        #endregion



        #region Common

        void DrawLevel(Level level) {
            for (int i = 0; i < level.cells.Count; i++) {
                string name = ((SegmentType)level.cells[i]).ToString();
                Sprite sprite; _dictIcons.TryGetValue("icon" + name, out sprite);
                _gridCells[i].sprite = sprite;
                _gridCells[i].name = name;
            }
        }

        bool CheckFileName(string name) {
            if (name == "sampleLevel") return false;
            if (name.Contains(".")) return false;
            if (name.Contains("\\")) return false;
            return true;
        }

        #endregion

        void EnableMoveCamera() {
            GlobalData.Instance.isEnableEdit = false;
            ResetButtons();
            _moveCameraButton.interactable = false;
        }

        void ResetButtons() {
            for (int i = 0; i < _container.childCount; i++) {
                _container.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }

        void LoadLevel() {
            if (File.Exists(Application.persistentDataPath + "/Levels/" + _inputLevelTitle.text + ".json")) {
                level = JsonUtility.FromJson<Level>(File.ReadAllLines(Application.persistentDataPath + "/Levels/" + _inputLevelTitle.text + ".json")[0]);
                _messageText.text = level.title + " loaded";
                DrawLevel(level);
            } else {
                _messageText.text = "file not exist";
            }
        }

        void SaveLevel() {
            if (CheckFileName(_messageText.text)) {
                for (int i = 0; i < level.cells.Count; i++) {
                    SegmentType cellName = (SegmentType)Enum.Parse(typeof(SegmentType), _gridCells[i].name);
                    level.cells[i] = (int)cellName;
                }
                List<string> saveFile = new List<string>();
                saveFile.Add(JsonUtility.ToJson(level));
                File.WriteAllLines(Application.persistentDataPath + "/Levels/" + level.title + ".json", saveFile.ToArray());
                _messageText.text = level.title + " saved";
            } else {
                _messageText.text = "choose another name";
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (!GlobalData.Instance.isEnableEdit) return;
            if (eventData.pointerEnter.transform.parent == _gridContainer) {
                if (eventData.button == PointerEventData.InputButton.Left) {
                    eventData.pointerEnter.GetComponent<Image>().sprite = _newCellSprite;
                }
                if (eventData.button == PointerEventData.InputButton.Right) {
                    eventData.pointerEnter.GetComponent<Image>().sprite = _defaultCellSprite;
                }
                Debug.Log("Pressed to " + eventData.pointerEnter.name);
            }
        }
    }
}
