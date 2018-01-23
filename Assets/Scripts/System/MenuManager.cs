using BugRush.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [Header("Buttons")]
    [SerializeField]
    private Button _quickGameButton = null;
    [SerializeField]
    private Button _editorButton = null;
    [SerializeField]
    private Button _quitButton = null;

    [Header("CommonUI")]
    [SerializeField]
    private Slider _rowsCountSlider;
    [SerializeField]
    private Slider _columnsCountSlider;
    [SerializeField]
    private InputField _levelTitleInput;
    [SerializeField]
    private Text _rowsValue;
    [SerializeField]
    private Text _columnsValue;

    void Start() {
        _quickGameButton.onClick.AddListener(QuickGame);
        _editorButton.onClick.AddListener(RunEditor);
        _quitButton.onClick.AddListener(delegate { Application.Quit(); });
        _rowsCountSlider.onValueChanged.AddListener(delegate { _rowsValue.text = (_rowsCountSlider.value*50).ToString(); });
        _columnsCountSlider.onValueChanged.AddListener(delegate { _columnsValue.text = (_columnsCountSlider.value*50).ToString(); });
    }

    void QuickGame() {
        GameController.Instance.InitLevel("sampleLevel");
        Debug.Log("run quick game");
        SceneManager.LoadScene("playmode");
    }

    void RunEditor() {
        if (_levelTitleInput.text != "" && _levelTitleInput.text != "sampleLevel") {
            GameController.Instance.InitLevel(int.Parse(_rowsValue.text), int.Parse(_columnsValue.text), _levelTitleInput.text);
            Debug.Log("run editor");
            SceneManager.LoadScene("editmode");
        }
    }

}
