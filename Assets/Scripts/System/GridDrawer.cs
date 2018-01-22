using BugRush.Controllers;
using BugRush.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Level {
    public string title;
    public int columns;
    public List<int> cells;

    public Level(int x, int y, string name) {
        title = name;
        columns = x;
        cells = new List<int>();
        List<int> temp = new List<int>();
        for (int j = 0; j < y; j++) { temp.Add(0); }
        for (int i = 0; i < x; i++) { cells.AddRange(temp); }
    }
}

namespace BugRush.System {
    public class GridDrawer : MonoBehaviour {

        public GridSegment gridsample;
        public Vector2 gridSize = new Vector2(50, 50);
        public float cellSize = 2.5f;
        public string levelName = "level_0";

        private Level _playLevel;

        void Start() {
            _playLevel = GameController.Instance.LoadLevel(levelName);
            DrawLevel(_playLevel);
            gridsample.gameObject.SetActive(false);
        }

        public void DrawLevel(Level level) {
            for (int i = 0; i < level.cells.Count; i += level.columns) {
                for (int j = 0; j < level.cells.Count / level.columns; j++) {
                    GameObject cell = Instantiate(gridsample.gameObject, transform);
                    cell.transform.localPosition = new Vector3(i / level.columns * cellSize, 0, j * cellSize);
                    cell.SetActive(true);
                    if (i / level.columns == 0 || j == 0 || i / level.columns == (gridSize.x - 1) || j == (gridSize.y - 1)) {
                        cell.GetComponent<GridSegment>().SetType(SegmentType.Border);
                        //_playLevel.cells[i][j] = 5;
                    } else {
                        cell.GetComponent<GridSegment>().SetType((SegmentType)level.cells[i + j]);
                    }
                }
            }
            //GameController.Instance.SaveLevel(_playLevel, levelName);
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
