using BugRush.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BugRush.System {
    public class SplashLoading : MonoBehaviour {

        [SerializeField]
        private Database _database;
        [SerializeField]
        private GameController _controller;

        void Start() {
            GameObject tmp = Instantiate(_database.gameObject);
            tmp.name = "Database";
            DontDestroyOnLoad(tmp);
            tmp = Instantiate(_controller.gameObject);
            tmp.name = "GameController";
            DontDestroyOnLoad(tmp);

            SceneManager.LoadScene("menu");
        }

    }
}
