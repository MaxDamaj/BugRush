using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum SegmentType {
    Default = 0,
    Spawner = 1,
    Hole = 2,
    Landmine = 3,
    Rocks = 4,
    Border = 5
};

namespace BugRush.System {
    public class GridSegment : MonoBehaviour {

        public SegmentType type;

        void Start() {
            SetType(type);
        }

        public void SetType(SegmentType newType) {
            if (transform.childCount > 0) { Destroy(transform.GetChild(0).gameObject); }
            type = newType;
            Instantiate(Resources.Load<GameObject>("Segments/seg" + type.ToString()), transform);
        }


    }
}