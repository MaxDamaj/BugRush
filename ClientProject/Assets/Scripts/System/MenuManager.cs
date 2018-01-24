using BugRush.Controllers;
using BugRush.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class UILevelRow {
    public Text titleText;
    public Button playButton;
    public GameObject instance;

    public UILevelRow(GameObject sample, RectTransform parent, string levelTitle) {
        instance = GameObject.Instantiate(sample, parent);
        titleText = instance.GetComponentInChildren<Text>();
        titleText.text = levelTitle;
        playButton = instance.GetComponentInChildren<Button>();
        playButton.onClick.AddListener(delegate { MenuManager.Instance.levelTitle = titleText.text; MenuManager.Instance.PlayGame(); });
    }

}

namespace BugRush.UI {
    public class MenuManager : MonoBehaviour {

        [Header("Buttons")]
        [SerializeField]
        private Button _quickGameButton = null;
        [SerializeField]
        private Button _quitButton = null;

        [Header("CommonUI")]
        [SerializeField]
        private RectTransform _levelsContainer = null;
        [SerializeField]
        private GameObject _levelRowSample = null;
        [HideInInspector]
        public string levelTitle;

        private static MenuManager instance;

        //Instance
        public static MenuManager Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<MenuManager>();
                }
                return instance;
            }
        }

        void Start() {
            levelTitle = "sampleLevel";
            _quickGameButton.onClick.AddListener(PlayGame);
            _quitButton.onClick.AddListener(delegate { Application.Quit(); });

            string[] files = GameController.Instance.GetLevels();
            
            for (int i = 0; i < files.GetLength(0); i++) {
                files[i] = files[i].Split('\\')[1];
                files[i] = files[i].Split('.')[0];
                new UILevelRow(_levelRowSample, _levelsContainer, files[i]);
            }
            _levelRowSample.SetActive(false);
        }



        public void PlayGame() {
            GameController.Instance.InitLevel(levelTitle);
            Debug.Log("run quick game");
            SceneManager.LoadScene("playmode");
        }



    }
}
