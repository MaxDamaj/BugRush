using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugRush.System {
    public class GridDrawer : MonoBehaviour {

        public GridSegment gridsample;
        public Transform gridContainer;
        public Vector2 gridSize = new Vector2(50, 50);
        public float cellSize = 2.5f;

        private Level _playLevel;

        public void Init(Level level) {
            _playLevel = level;
            DrawLevel(_playLevel);
            gridsample.gameObject.SetActive(false);
        }

        public void DrawLevel(Level level) {
            for (int i = 0; i < level.cells.Count; i += level.columns) {
                for (int j = 0; j < level.cells.Count / level.rows; j++) {
                    GameObject cell = Instantiate(gridsample.gameObject, gridContainer);
                    cell.transform.localPosition = new Vector3(i / level.columns * cellSize, 0, j * cellSize);
                    cell.SetActive(true);
                    cell.GetComponent<GridSegment>().SetType((SegmentType)level.cells[i + j]);
                }
            }
        }

        public void ClearGrid() {
            int count = gridContainer.childCount;
            for (int i = 0; i < count; i++) {
                Destroy(gridContainer.GetChild(i).gameObject);
            }
        }

        void FixedUpdate() {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                if (Input.GetMouseButtonDown(0) && GlobalData.Instance.isEnableEdit) {
                    if (objectHit.name.Contains("seg")) {
                        objectHit.parent.GetComponent<GridSegment>().SetType(GlobalData.Instance.newSegmentType);
                    }
                }
            }
        }

    }
}
