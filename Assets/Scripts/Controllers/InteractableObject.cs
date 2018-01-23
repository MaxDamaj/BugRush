using BugRush.Controllers;
using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType {
    Unsigned = 0,
    Landmine = 1,
    RepairKit = 2
}

namespace BugRush.Controllers {
    public class InteractableObject : MonoBehaviour {

        public ObjectType type;
        public bool isInteractWithEnemies;
        public string destroyFX;

        void OnCollisionEnter(Collision coll) {
            if (coll.gameObject.tag == "Player") {
                switch (type) {
                    case ObjectType.Landmine:
                        GlobalData.Instance.Health -= GlobalData.Instance.level.landminePower;
                        break;
                    case ObjectType.RepairKit:
                        GlobalData.Instance.Health += GlobalData.Instance.level.repairKitPower;
                        break;
                }
                GameController.Instance.SpawnFX(gameObject, destroyFX, true);
            }
            if (coll.gameObject.tag == "Enemy" && isInteractWithEnemies) {
                switch (type) {
                    case ObjectType.Landmine:
                        coll.gameObject.GetComponent<EnemyController>().DecreaseHealth(GlobalData.Instance.level.landminePower);
                        break;
                }
                GameController.Instance.SpawnFX(gameObject, destroyFX, true);
            }
        }
    }
}
