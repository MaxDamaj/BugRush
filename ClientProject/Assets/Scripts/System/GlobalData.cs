using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BugRush.System {
    public class GlobalData : ScriptableObject {

        public float Health { get { return health; } set {
                health = value;
                if (value < 0) { health = 0; }
                if (OnValueUpdate != null) OnValueUpdate();
            } }
        public float Energy { get { return energy; } set {
                energy = value;
                if (value < 0) { energy = 0; }
                if (OnValueUpdate != null) OnValueUpdate();
            } }
        public int EnemiesCount {
            get { return enemiesCount; }
            set {
                enemiesCount = value;
                if (value < 0) { enemiesCount = 0; }
                if (OnValueUpdate != null) OnValueUpdate();
            }
        }

        public Level level;
        public bool isEnableEdit;

        private float health;
        private float energy;
        private int enemiesCount;

        private static GlobalData globalData;
        public static event UnityAction OnValueUpdate;

#if UNITY_EDITOR
        [MenuItem("Assets/Create GlobalData asset")]
        private static void CreateGDContainer() {
            var obj = ScriptableObject.CreateInstance<GlobalData>();
            AssetDatabase.CreateAsset(obj, "Assets/Resources/GlobalData.asset");
            AssetDatabase.SaveAssets();
        }
#endif

        public static GlobalData Instance {
            get {
                if (globalData == null) {
                    globalData = Resources.Load<GlobalData>("GlobalData");
                }
                return globalData;
            }
        }
    }
}
