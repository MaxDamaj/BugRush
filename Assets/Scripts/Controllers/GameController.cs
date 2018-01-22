using BugRush.System;
using BugRush.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BugRush.Controllers {
    public class GameController : MonoBehaviour {

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
            FindObjectOfType<UIPlayScreen>().Init(Database.Instance.GetVehicle());
        }

        #region Methods

        public Level LoadLevel(string title) {
            if (Directory.Exists(Application.persistentDataPath + "/Levels")) {
                if (File.Exists(Application.persistentDataPath + "/Levels/" + title)) {
                    List<string> saveFile = new List<string>();
                    saveFile.AddRange(File.ReadAllLines(Application.persistentDataPath + "/Levels/" + title));
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
                File.WriteAllLines(Application.persistentDataPath + "/Levels/" + title, saveFile.ToArray());
            } else {
                Directory.CreateDirectory(Application.persistentDataPath + "/Levels");
            }
        }

        public void SpawnFX(GameObject target, string fxTitle, bool isDestroyTarget) {
            GameObject fx = Instantiate(Resources.Load<GameObject>("FXs/" + fxTitle), target.transform.position, target.transform.rotation);
            Destroy(fx, 3);
            if (isDestroyTarget) Destroy(target);
        }

        #endregion

    }
}
