using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Passive = 0,
    Fleeing = 1,
    Hunting = 2
}

namespace BugRush.Controllers {
    public class EnemyController : MonoBehaviour {

        public EnemyType enemyType;
        public float maxSpeed = 4f;
        public float maxHP = 50f;
        public TextMesh hpValue;
        public string destroyFX;

        private Rigidbody _rigidbody;
        private Transform _target;
        private bool isMoving;
        private float _health;

        void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _health = maxHP;
            hpValue.text = _health + "/" + maxHP;
        }

        void FixedUpdate() {
            if (_target == null) return;
            if (isMoving && enemyType != EnemyType.Passive) {
                Vector3 velocity = Vector3.zero;
                if (enemyType == EnemyType.Fleeing) velocity = new Vector3(transform.position.x - _target.position.x, 0, transform.position.z - _target.position.z);
                if (enemyType == EnemyType.Hunting) velocity = new Vector3(_target.position.x - transform.position.x, 0, _target.position.z - transform.position.z);
                if (_rigidbody.velocity.magnitude < maxSpeed) {
                    _rigidbody.AddRelativeForce(velocity, ForceMode.Force);
                }
            }
            hpValue.text = _health + "/" + maxHP;
        }

        #region Colliders

        void OnCollisionEnter(Collision coll) {
            if (coll.gameObject.tag == "Player") {
                DecreaseHealth(coll.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
            }
        }

        void OnTriggerEnter(Collider coll) {
            if (coll.gameObject.tag == "Player") {
                isMoving = true;
                _target = coll.transform;
            }
        }
        void OnTriggerExit(Collider coll) {
            if (coll.gameObject.tag == "Player") {
                isMoving = false;
            }
        }

        #endregion

        public void DecreaseHealth(float value) {
            _health -= (value);
            _health = Mathf.RoundToInt(_health);
            if (_health <= 0) {
                _health = 0;
                GameController.Instance.SpawnFX(gameObject, destroyFX, true);
            }
        }
    }
}
