using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugRush.System {
    public class GridDrawer : MonoBehaviour {

        public GridSegment gridsample;
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
                for (int j = 0; j < level.cells.Count / level.columns; j++) {
                    GameObject cell = Instantiate(gridsample.gameObject, transform);
                    cell.transform.localPosition = new Vector3(i / level.columns * cellSize, 0, j * cellSize);
                    cell.SetActive(true);
                    cell.GetComponent<GridSegment>().SetType((SegmentType)level.cells[i + j]);
                }
            }
        }

        void FixedUpdate() {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                if (Input.GetMouseButtonDown(0)) Debug.Log("Hit to " + objectHit.name);
            }
        }

    }
}
