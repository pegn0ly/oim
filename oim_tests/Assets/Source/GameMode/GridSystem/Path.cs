using System;
using System.Collections.Generic;
using UnityEngine;

namespace OIMField
{
	//класс, используемый для расчета путей на сетке.
    public static class PathFinder
	{
		// для заданной точки point сетки grid находит ближайшую из доступных точек reachable_point
		static private int GetClosestPointToPoint(CellGrid grid, int point, List<int> reachable_points)
        {
			double ClosestDist = Double.MaxValue;
			int CurrentPoint = -1;
			foreach (int rp in reachable_points)
			{
				Vector3 Coords = grid.PointToCoords(point);
				Vector3 pCoords = grid.PointToCoords(rp);
				double CurrDist = Math.Sqrt(Math.Pow((Coords.x - pCoords.x), 2) + Math.Pow((Coords.z - pCoords.z), 2));
				if (CurrDist <= ClosestDist)
				{
					ClosestDist = CurrDist;
					CurrentPoint = rp;
				}
			}
			return CurrentPoint;
		}

		// для заданной точки point сетки grid возвращает список точек, достижимых из нее, отбрасывая уже пройденные точки checked_points
		static private List<int> GetPointsReachableFromThis(CellGrid grid, int point, List<int> checked_points)
        {
			List<int> FoundedPoints = new List<int>();
			// восстановить координаты точки, чтобы найти координаты, по которым будут находиться соседние точки
			Vector3 PointCoords = grid.PointToCoords(point);
			for (int i = ((int)PointCoords.x / CellGrid.CellSize - 1); i <= ((int)PointCoords.x / CellGrid.CellSize + 1); i++)
			{
				for (int j = ((int)PointCoords.z / CellGrid.CellSize  - 1); j <= ((int)PointCoords.z / CellGrid.CellSize + 1); j++)
				{
					// координаты не должны выходить за пределы размеров поля
					if ((i >= 0 && j >= 0) && i < grid.Dimensions.Width && j < grid.Dimensions.Height)
					{
						int PossiblePoint = grid.Dimensions.Width * j + i;
						// также отбрасываются непроходимые и уже пройденные точки.
						if (grid.IsValidCell(PossiblePoint) &&
							grid.IsPassableCell(PossiblePoint) &&
							checked_points.Contains(PossiblePoint) == false)
						{
							FoundedPoints.Add(PossiblePoint);
						}
					}
				}
			}
			return FoundedPoints;
		}

		// возвращает список связей между клетками сетки grid, при условии, что в списке содержится путь между клетками start_point и end_point
        static public SortedList<int, int> FindPath(CellGrid grid, int start_point, int end_point)
        {
			if(!grid.IsValidCell(end_point) || !grid.IsPassableCell(end_point))
			{
				return null;
			}
			//
			List<int> ReachablePoints = new List<int>();
			List<int> CheckedPoints = new List<int>();
			SortedList<int, int> Connections = new SortedList<int, int>();
			// стартовая точка, очевидно, является изначально достижимой
			ReachablePoints.Add(start_point);
			Connections.Add(start_point, -1);
			Connections.Add(-1, end_point);
			// непосредственно поиск
			while (ReachablePoints.Count != 0)
			{
				// выбрать ближайшую к конечной точку из достижимых
				int Point = GetClosestPointToPoint(grid, end_point, ReachablePoints);
				// добавить ее в список проверенных и убрать из списка достижимых
				CheckedPoints.Add(Point);
				ReachablePoints.Remove(Point);
				// найти точки, достижимые из выбранной
				List<int> PossiblePoints = GetPointsReachableFromThis(grid, Point, CheckedPoints);
				// если таких точек нет, проверяем следующую
				if (PossiblePoints.Count == 0)
				{
					continue;
				}
				// если среди них есть конечная - завершить алгоритм
				else if (PossiblePoints.Contains(end_point))
				{
					Connections.Add(end_point, Point);
					return Connections;
				}
				// иначе добавить все найденные точки в список достижимых для дальнейшей проверки
				else
				{
					foreach (int pp in PossiblePoints)
					{
						if (ReachablePoints.Contains(pp) == false)
						{
							ReachablePoints.Add(pp);
							Connections.Add(pp, Point);
						}
					}
				}
			}
			return null;
		}

		static public List<Vector3> RestorePath(CellGrid grid, SortedList<int, int> connections)
		{
			List<Vector3> Moves = new List<Vector3>();
            int prev = connections[-1];
            do
            {
                Moves.Add(grid.PointToCoords(prev));
                prev = connections[prev];
            } while (prev != -1);
			Moves.Reverse();
			return Moves;
		}
    }
}

