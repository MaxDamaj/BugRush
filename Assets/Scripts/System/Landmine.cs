using BugRush.Controllers;
using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour {

    public float damageValue = 50f;
    public string destroyFX;

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Player") {
            GlobalData.Instance.Health -= damageValue;
            GameController.Instance.SpawnFX(gameObject, destroyFX, true);
        }
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<EnemyController>().DecreaseHealth(damageValue);
            GameController.Instance.SpawnFX(gameObject, destroyFX, true);
        }
    }
}
