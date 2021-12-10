using System.Collections.Generic;

using UnityEngine;

using OIMWeb;

namespace OIMField
{ 
    // отвечает за размещение препятствий на поле
    public class ObstaclePlacer : MonoBehaviour 
    {
        private CellGrid GridToPlace;

        private Dictionary<int, GameObject> ObstacleMap = new Dictionary<int, GameObject>();

        private void Awake() 
        {
            GridToPlace = (CellGrid)gameObject.GetComponent(typeof(CellGrid));
        }

        private void Start() 
        {
            
        }

        public void GenerateObstacles(GridProps props)
        {
            for (int i = 0; i < props.Width; i++)
            {
                for (int j = 0; j < props.Height; j++)
                {
                    if(Random.Range(0, 3) == 1)
                    {
                        GridToPlace.SetPassability(j * props.Width + i, false);
                        GameObject Obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        ObstacleMap.Add(j * props.Width + i, Obstacle);
                        Obstacle.transform.position = new Vector3((props.BaseX + CellGrid.CellSize * i) + CellGrid.CellSize / 2, 0.01f, (props.BaseY + CellGrid.CellSize * j) + CellGrid.CellSize / 2);
                        Obstacle.transform.localScale = new Vector3(CellGrid.CellSize / 2, CellGrid.CellSize / 2, CellGrid.CellSize / 2);
                        Obstacle.transform.SetParent(transform);
                    }
                }
            }
        }
    }
}