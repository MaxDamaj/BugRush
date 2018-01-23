using BugRush.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BugRush.UI {
    public class UIPlayScreen : MonoBehaviour {

        [SerializeField]
        private Image _playerHPImage = null;
        [SerializeField]
        private Image _playerEnergyImage = null;
        [SerializeField]
        private Text _enemiesCounterText = null;

        private Vehicle _vehicle;


        void Start() {
            _vehicle = Database.Instance.GetVehicle();
            GlobalData.OnValueUpdate += RefreshUI;
        }

        public void RefreshUI() {
            _playerHPImage.fillAmount = GlobalData.Instance.Health / _vehicle.maxHP;
            _playerEnergyImage.fillAmount = GlobalData.Instance.Energy / _vehicle.maxEnergy;
            _enemiesCounterText.text = GlobalData.Instance.EnemiesCount.ToString();
        }

        void OnDestroy() {
            GlobalData.OnValueUpdate -= RefreshUI;
        }
    }
}
