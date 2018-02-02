using BugRush.Controllers;
using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugRush.Controllers {
    public class BugsSpawner : MonoBehaviour {

        [SerializeField]
        private GameObject _body;

        private List<int> spawnQuantity;

        void Start() {
            spawnQuantity = new List<int>();
            for (int i = 0; i < GlobalData.Instance.level.spawnBugsQuantity.Count; i++) {
                spawnQuantity.Add(GlobalData.Instance.level.spawnBugsQuantity[i]);
            }
            IEnumerator spawn = SpawnBugs();
            StartCoroutine(spawn);
        }

        IEnumerator SpawnBugs() {
            while (true) {
                if (!GlobalData.Instance.isEnableEdit) {
                    bool isSpawn = false;
                    Vector3 rndVector = Vector3.zero;
                    for (int i = 0; i < Database.Instance.enemiesTypes.Count; i++) {
                        if (spawnQuantity[i] > 0) {
                            isSpawn = true;
                            rndVector = new Vector3(Random.Range(-10, 10), 2.5f, Random.Range(-GlobalData.Instance.level.spawnRadius, GlobalData.Instance.level.spawnRadius));
                            GameController.Instance.SpawnEnemy(transform.position + rndVector, i);
                            spawnQuantity[i]--;
                        }
                    }
                    if (!isSpawn) { Destroy(_body); }
                }
                yield return new WaitForSeconds(GlobalData.Instance.level.spawnDelay);
            }
        }
    }
}
