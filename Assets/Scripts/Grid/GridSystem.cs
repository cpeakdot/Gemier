using UnityEngine;

namespace Gemier.GridSpace
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private GameObject gridVisualPrefab;
        [SerializeField] private bool enableGridCreationOnRuntime = false;

        Grid[,] gridArray;

        private void Start()
        {
            if(enableGridCreationOnRuntime)
            {
                InitGrid();
            }
        }

        /// <summary>
        /// Initializes the grid based on the width and height values.
        /// </summary>
        /// <param name="clearAll"></param>
        [ExecuteInEditMode]
        public void InitGrid(bool clearAll = false)
        {
            /// If already created in inspector
            ClearGrid(clearAll);

            gridArray = new Grid[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Grid newGrid = new Grid(x, z, this);

                    gridArray[x, z] = newGrid;

                    GameObject gridVisual = Instantiate(gridVisualPrefab, GetWorldPositionOfAGrid(newGrid), Quaternion.identity);

                    newGrid.SetVisual(gridVisual);

                    gridVisual.name = newGrid.ToString();
                }
            }
        }

        /// <summary>
        /// Clears Grids that already been created.
        /// </summary>
        /// <param name="clearAll"> Clears all grids on the scene </param>
        [ExecuteInEditMode]
        public void ClearGrid(bool clearAll = false)
        {
            if(clearAll)
            {
                var grids = GameObject.FindObjectsOfType<GridGemController>();

                if(grids != null && grids.Length > 0)
                {
                    foreach (var grid in grids)
                    {
                        DestroyImmediate(grid.gameObject);
                    }
                }

                gridArray = null;
                
                return;
            }

            if(gridArray != null && gridArray.Length > 0)
            {
                foreach (var grid in gridArray)
                {
                    DestroyImmediate(grid.GetVisual);
                }
                gridArray = null;
            }
        }

        /// <summary>
        /// Clears all grids on the scene, before initializing this one.
        /// </summary>
        [ExecuteInEditMode]
        public void InitGridClear()
        {
            InitGrid(true);
        }

        /// <summary>
        /// Clears all grids on the scene.
        /// </summary>
        [ExecuteInEditMode]
        public void ClearAllGrids()
        {
            var grids = GameObject.FindObjectsOfType<GridGemController>();

                if(grids != null && grids.Length > 0)
                {
                    foreach (var grid in grids)
                    {
                        DestroyImmediate(grid.gameObject);
                    }
                }
        }

        public Vector3 GetWorldPositionOfAGrid(Grid grid)
        {
            Vector3 gridPosition = new Vector3(grid.GetX, 0, grid.GetZ);
            return this.transform.position + (gridPosition * cellSize);
        }

        public Grid GetGrid(Vector3 worldPosition)
        {
            var x = Mathf.FloorToInt(worldPosition.x / cellSize);

            var z = Mathf.FloorToInt(worldPosition.z / cellSize);

            return gridArray[x, z];
        }
    }
}

