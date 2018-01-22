using BugRush.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Vehicle {
    public string Title;
    public float maxHP;
    public float maxEnergy;
    public float maxSpeed;
    public float energyConsuming;

    public VehicleObject vehicle {
        get {
            return Resources.Load<VehicleObject>("Vehicles/veh" + Title);
        }
    }
}

namespace BugRush.System {
    public class Database : MonoBehaviour {

        [SerializeField]
        private Vehicle _vehicle = null;

        private static Database instance;


        //Instance
        public static Database Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<Database>();
                }
                return instance;
            }
        }

        public Vehicle GetVehicle() {
            return _vehicle;
        }

    }
}
