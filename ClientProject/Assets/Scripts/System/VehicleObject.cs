using System.Collections;
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
                GlobalData.Instance.Health -= collision.impulse.magnitude/4;
            }
            if (collision.gameObject.tag == "Void") {
                GlobalData.Instance.Health -= int.MaxValue;
            }
        }

    }
}
