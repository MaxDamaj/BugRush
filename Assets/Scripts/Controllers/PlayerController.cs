using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugRush.Controllers {
    public class PlayerController : MonoBehaviour {

        [SerializeField]
        private Transform _cameraPoint = null;
        [SerializeField]
        private Transform _startPoint = null;
        [SerializeField]
        private string destroyFX = "fx_destroy_blue";

        private Vehicle _vehicle;
        private VehicleObject _player;
        private float _maxSpeed;
        private Rigidbody _rigidbody;

        void Start() {
            _vehicle = Database.Instance.GetVehicle();
            _player = Instantiate(_vehicle.vehicle, _startPoint.position, _startPoint.rotation);
            _maxSpeed = _vehicle.maxSpeed;
            _rigidbody = _player.GetComponent<Rigidbody>();
            GlobalData.Instance.Health = _vehicle.maxHP;
            GlobalData.Instance.Energy = _vehicle.maxEnergy;
            GlobalData.OnValueUpdate += DeathCheck;
        }

        void FixedUpdate() {
            if (_player == null) return;
            _cameraPoint.position = _player.transform.position;
            //Moving
            Vector3 velocity = new Vector3(VirtualJoystick.Instance.Horizontal(), 0, VirtualJoystick.Instance.Vertical());
            if (_rigidbody.velocity.magnitude < _maxSpeed && GlobalData.Instance.Energy > 0) {
                _rigidbody.AddRelativeForce(velocity, ForceMode.VelocityChange);
                GlobalData.Instance.Energy -= velocity.sqrMagnitude / 100f;
            }
            //Player body object look at movement position
            _player.mark.localPosition = velocity;
            _player.playerBody.LookAt(_player.mark);
        }

        private void OnDestroy() {
            GlobalData.OnValueUpdate -= DeathCheck;
        }

        void DeathCheck() {
            if (GlobalData.Instance.Health <= 0) {
                GameController.Instance.SpawnFX(_player.gameObject, destroyFX, true);
            }
        }

    }
}
