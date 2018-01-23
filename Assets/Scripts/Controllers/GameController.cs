using BugRush.System;
using BugRush.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Level {
    public string title;
    public int rows;
    public int columns;
    public List<int> cells;
    public float landminePower;
    public float repairKitPower;
    public int spawnDelay;
    public float spawnRadius;
    public List<int> spawnBugsQuantity;

    public Level(int x, int y, string name) {
        title = name;
        rows = x;
        columns = y;
        landminePower = 50f;
        repairKitPower = 25f;
        spawnDelay = 10;
        spawnRadius = 7f;
        spawnBugsQuantity = new List<int>();
        spawnBugsQuantity.Add(10); //small bugs
        spawnBugsQuantity.Add(3);  //middle bugs

        cells = new List<int>();
        List<int> temp = new List<int>();
        for (int j = 0; j < y; j++) { temp.Add(0); }
        for (int i = 0; i < x; i++) { cells.AddRange(temp); }
    }
}

namespace BugRush.Controllers {
    public class GameController : MonoBehaviour {

        [SerializeField]
        private GridDrawer _gridDrawer = null;
        [SerializeField]
        private PlayerController _playerController = null;

        private string levelName;
        private Transform _fxContainer;
        private Transform _entitiesContainer;

        private static GameController instance;

        //Instance
        public static GameController Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<GameController>();
                }
                return instance;
            }
        }

        private void Start() {
            _fxContainer = GameObject.Find("FxContainer").transform;
            _entitiesContainer = GameObject.Find("EntitiesContainer").transform;
        }


        #region Level

        public void InitLevel(string level) {
            levelName = level;
            GlobalData.Instance.level = LoadLevel(levelName);
            _gridDrawer.Init(GlobalData.Instance.level);
            _playerController.Init();
        }

        public void InitLevel(int row, int columns, string title) {
            levelName = title;
            GlobalData.Instance.level = new Level(row, columns, title);
            _gridDrawer.Init(GlobalData.Instance.level);
            _playerController.Init();
        }

        public void UnloadLevel() {
            //Remove Enemies
            int count = _entitiesContainer.childCount;
            for (int i=0; i<count; i++) {
                Destroy(_entitiesContainer.GetChild(i).gameObject);
            }
            //Remove cells
            _gridDrawer.ClearGrid();
            GlobalData.Instance.isEnableEdit = false;
            Debug.Log("return to menu");
            SceneManager.LoadScene("menu");
        }

        public Level LoadLevel(string title) {
            if (Directory.Exists(Application.persistentDataPath + "/Levels")) {
                List<string> saveFile = new List<string>();
                if (File.Exists(Application.persistentDataPath + "/Levels/" + title + ".json")) {
                    saveFile.AddRange(File.ReadAllLines(Application.persistentDataPath + "/Levels/" + title + ".json"));
                    return JsonUtility.FromJson<Level>(saveFile[0]);
                } else {
                    saveFile.Add(Resources.Load<TextAsset>("sampleLevel").text);
                    return JsonUtility.FromJson<Level>(saveFile[0]);
                }
            } else {
                Directory.CreateDirectory(Application.persistentDataPath + "/Levels");
            }
            return new Level(50, 50, title);
        }

        public void SaveLevel(Level level, string title) {
            if (Directory.Exists(Application.persistentDataPath + "/Levels")) {
                List<string> saveFile = new List<string>();
                saveFile.Add(JsonUtility.ToJson(level));
                File.WriteAllLines(Application.persistentDataPath + "/Levels/" + title + ".json", saveFile.ToArray());
            } else {
                Directory.CreateDirectory(Application.persistentDataPath + "/Levels");
            }
        }

        #endregion

        public void SpawnFX(GameObject target, string fxTitle, bool isDestroyTarget) {
            GameObject fx = Instantiate(Resources.Load<GameObject>("FXs/" + fxTitle), target.transform.position, target.transform.rotation);
            fx.transform.SetParent(_fxContainer);
            Destroy(fx, 3);
            if (isDestroyTarget) Destroy(target);
        }

        public void SpawnEnemy(Vector3 position, int enemyID) {
            GameObject enemy = Instantiate(Resources.Load<GameObject>("Enemies/" + Database.Instance.enemiesTypes[enemyID]), position, Quaternion.identity);
            enemy.transform.SetParent(_entitiesContainer);
            RecalculateEnemies();
        }

        public VehicleObject SpawnPlayer(Vehicle vehicle) {
            VehicleObject player = Instantiate(vehicle.vehicle, _entitiesContainer);
            return player;
        }

        public void RecalculateEnemies() {
            GlobalData.Instance.EnemiesCount = _entitiesContainer.childCount-1;
            if (GlobalData.Instance.EnemiesCount == 0) {
                UnloadLevel();
            }
        }

        

    }
}
