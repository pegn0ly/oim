using UnityEngine;

namespace OIMField
{    
    // основной класс, представляющий поле боя
    public class Field : MonoBehaviour 
    {
        private CellGrid Grid;
        private ClickDetector Detector;
        private FieldVisualizer Visualizer;
        private ObstaclePlacer ObstaclePlacer;
        public void InitField(GridProps props)
        {
            Grid = (CellGrid)gameObject.AddComponent(typeof(CellGrid));            
            Detector = (ClickDetector)gameObject.AddComponent(typeof(ClickDetector));
            Visualizer = (FieldVisualizer)gameObject.AddComponent(typeof(FieldVisualizer));
            ObstaclePlacer = (ObstaclePlacer)gameObject.AddComponent(typeof(ObstaclePlacer));

            Grid.Create(props);
            Visualizer.Render(props);
            ObstaclePlacer.GenerateObstacles(props);
        }
    }
}