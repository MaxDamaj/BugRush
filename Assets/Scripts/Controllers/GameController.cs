using BugRush.System;
using BugRush.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;


[Serializable]
public class Level {
    public string title;
    public int columns;
    public List<int> cells;
    public float landminePower;
    public int spawnDelay;
    public float spawnRadius;
    public List<int> spawnBugsQuantity;

    public Level(int x, int y, string name) {
        title = name;
        columns = x;
        landminePower = 50f;
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
        private string levelName = "level_0";

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
            GlobalData.Instance.level = LoadLevel(levelName);
            _gridDrawer.Init(GlobalData.Instance.level);
            FindObjectOfType<UIPlayScreen>().Init(Database.Instance.GetVehicle());
        }

        #region Methods

        public Level LoadLevel(string title) {
            if (Directory.Exists(Application.persistentDataPath + "/Levels")) {
                if (File.Exists(Application.persistentDataPath + "/Levels/" + title + ".lvl")) {
                    List<string> saveFile = new List<string>();
                    saveFile.AddRange(File.ReadAllLines(Application.persistentDataPath + "/Levels/" + title + ".lvl"));
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
                File.WriteAllLines(Application.persistentDataPath + "/Levels/" + title + ".lvl", saveFile.ToArray());
            } else {
                Directory.CreateDirectory(Application.persistentDataPath + "/Levels");
            }
        }

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

        public void RecalculateEnemies() {
            GlobalData.Instance.EnemiesCount = _entitiesContainer.childCount;
        }

        #endregion

    }
}
