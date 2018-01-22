using BugRush.Controllers;
using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugsSpawner : MonoBehaviour {

    public TextMesh[] spawnValuesText;

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
            Vector3 rndVector = Vector3.zero;
            for (int i = 0; i<Database.Instance.enemiesTypes.Count; i++) {
                if (spawnQuantity[i] > 0) {
                    rndVector = new Vector3(Random.Range(-10, 10), 2.5f, Random.Range(-GlobalData.Instance.level.spawnRadius, GlobalData.Instance.level.spawnRadius));
                    GameController.Instance.SpawnEnemy(transform.position + rndVector, i);
                    spawnQuantity[i]--;
                    spawnValuesText[i].text = spawnQuantity[i].ToString();
                }
            }
            yield return new WaitForSeconds(GlobalData.Instance.level.spawnDelay);
        }
    }
}
