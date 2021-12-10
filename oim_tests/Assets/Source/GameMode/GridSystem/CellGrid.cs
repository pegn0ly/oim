using UnityEngine;

using OIMFight;

namespace OIMField
{
    public struct GridProps
    {
        public int id;
        public int BaseX;
        public int BaseY;
        public int Width;
        public int Height;    
    }

    // класс, представляющий сетку поля с математической точки зрения.
    // Константы:
    // - CellSize - размер клетки поля относительно мировых координат
    // - INVALID_POINT - точка, которая гарантированно не является частью сетки
    // Поля:
    // - Passabilities - массив, хранящий информацию о проходимости точек поля
    // - Coords - массив, хранящий информацию о мировых координатах точек поля
    // - Dimensions - размерности сетки
    public class CellGrid : MonoBehaviour
    {
        public static int CellSize = 10;
        public static int INVALID_POINT = -1;

        private bool[] Passabilities; 
        private Vector3[] Coords;
        private GridProps _Dimensions;
        public GridProps Dimensions
        {
            get
            {
                return _Dimensions;
            }
            private set{}
        }

        // подписываемся на ивенты UnitDisplacer'а, чтобы постановка/перемещение юнитов влияли на проходимость точек поля
        private void Awake()
        {
            UnitDisplacer.UnitPlaced += UnitPlacedOnGrid;
            UnitDisplacer.MoveCompleted += UnitMovedOnGrid;
        }
        public void Create(GridProps props)
        {
            _Dimensions = props;
            Passabilities = new bool[props.Width * props.Height];
            Coords = new Vector3[props.Width * props.Height];
            for (int i = 0; i < props.Width; i++)
            {
                for (int j = 0; j < props.Height; j++)
                {
                    Passabilities[j * props.Width + i] = true;
                    Coords[j * props.Width + i] = new Vector3((props.BaseX + CellSize * i) + CellSize / 2, 0.01f, (props.BaseY + CellSize * j) + CellSize / 2);
                }
            }
        }
        // проверяет, входит ли клетка cell частью поля
        public bool IsValidCell(int cell)
        {
            return cell >= 0 && cell < (Dimensions.Width * Dimensions.Height);
        }
        // проверяет, проходима ли клетка cell
        public bool IsPassableCell(int cell)
        {
            return Passabilities[cell];
        }
        // устанавливает проходимость клетки cell
        public void SetPassability(int cell, bool passability)
        {
            Passabilities[cell] = passability;
        }
        // переводит клетку cell в ее мировые координаты
        public Vector3 PointToCoords(int cell)
		{
            return Coords[cell];
		}
		// для заданных мировых координат position возвращает точку point сетки, максимально близкую к данной позиции
		public int GetClosestPointToPosition(Vector3 position)
		{
			float x_coord = position.x / CellSize;
			float y_coord = position.z / CellSize;
			if(x_coord < Dimensions.BaseX || x_coord >= Dimensions.Width || y_coord < Dimensions.BaseY || y_coord >= Dimensions.Height)
			{
				return INVALID_POINT;
			}
			return (int)x_coord + (int)y_coord * Dimensions.Width;
		}
        // блокирует клетку при постановке в нее юнита
        private void UnitPlacedOnGrid(PlaceResult place_result)
        {
            Passabilities[place_result.Point] = false;
        }
        // обновляет проходимости стартовой и конечной клеток после завершения очередного перемещения юнита
        private void UnitMovedOnGrid(MovementResult move_result)
        {
            Passabilities[move_result.StartPoint] = true;
            Passabilities[move_result.DestinationPoint] = false;
        }
    }
}