using BugRush.Controllers;
using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour {

    public string destroyFX;

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Player") {
            GlobalData.Instance.Health -= GlobalData.Instance.level.landminePower;
            GameController.Instance.SpawnFX(gameObject, destroyFX, true);
        }
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<EnemyController>().DecreaseHealth(GlobalData.Instance.level.landminePower);
            GameController.Instance.SpawnFX(gameObject, destroyFX, true);
        }
    }
}
