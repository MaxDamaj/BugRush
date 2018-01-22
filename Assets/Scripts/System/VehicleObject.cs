﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugRush.System {
    public class VehicleObject : MonoBehaviour {

        [SerializeField]
        public Transform playerBody = null;
        [SerializeField]
        public Transform mark = null;

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.tag == "Enemy") {
                GlobalData.Instance.Health -= 1;
            }
        }

    }
}