using System.Collections.Generic;
using System.Collections;

using Newtonsoft.Json;

using UnityEngine;

using OIMField;

namespace OIMFight
{
    // форма информации о результате размещения юнита на сетке
    public struct PlaceResult
    {
        public GameObject UnitPlaced;
        public int Point;

        public PlaceResult(GameObject unit, int point)
        {
            UnitPlaced = unit;
            Point = point;
        }
    }
    // форма информации о результате перемещения юнита по сетке
    public struct MovementResult
    {
        public GameObject UnitDisplaced;
        public int StartPoint;
        public int DestinationPoint;
        public MovementResult(GameObject unit, int start_point, int destination_point)
        {
            UnitDisplaced = unit;
            StartPoint = start_point;
            DestinationPoint = destination_point;
        }
    }

    // класс, отвечающий за позиционирование и перемещение юнитов во время боя

    // Поля:
    // - Fight - ссылка на бой, компонентом которого является данный объект
    // - Grid - сетка, по которой должны перемещаться юниты
    // - UnitsPositions - текущее расположение юнитов по клеткам сетки
    // - CurrentUnitToMove - юнит, который должен перемещаться в данный момент

    // Ивенты:
    // - UnitPlaced - вызывается при постановке юнита на поле в фазе подготовки к бою, передается форма результата постановки
    // - MoveCompleted - вызывается при успешном перемещении юнита, передается форма результата перемещения
    // - PositionsUpdated - вызывается при необходимости обновить позиции юнитов в бэкенде(хочу убрать, но пока не придумал, как лучше сделать)

    public class UnitDisplacer :  MonoBehaviour
    {
        private Fight Fight;
        private CellGrid Grid;
        private Dictionary<GameObject, int> UnitsPositions = new Dictionary<GameObject, int>();
        private GameObject CurrentUnitToMove;
        
        public delegate void OnUnitPlacedDelegate(PlaceResult place_result);
        public static event OnUnitPlacedDelegate UnitPlaced;

        public delegate void OnMoveCompleted(MovementResult move_result);
        public static event OnMoveCompleted MoveCompleted;

        public delegate void OnPositionsUpdated(FightSavedCondition condition);
        public static event OnPositionsUpdated PositionsUpdated;

        private void Awake() 
        {
            Fight = (Fight)gameObject.GetComponent(typeof(Fight));
            Grid = (CellGrid)GameObject.FindObjectOfType(typeof(CellGrid));
            //
            OIMField.ClickDetector.OnClicked += PlaceUnit;
            FightSequencer.OnFightStageGhanged += FightStageUpdated;
            UnitSequencer.UnitSetAsCurrent += SetUnitToDisplace;
            FightRegistrator.TurnRegistered += SavePositionsUponNewTurnStarted;
            //
            MoveCompleted += UpdateUnitPosition;
            //
            transform.SetParent(transform);
        }

        // размещает юнита в ближайшей к position клетке поля, если точка валидна и проходима(только в стадии подготовки)
        private void PlaceUnit(Vector3 position)
        {
            RaycastHit Hit;
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out Hit))
            {
                if (Hit.point.x >= 0.0f && Hit.point.z >= 0)
                {
                    int cell = Grid.GetClosestPointToPosition(Hit.point);
                    if(Grid.IsValidCell(cell) && Grid.IsPassableCell(cell))
                    {
                        GameObject unit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        unit.name = "unit_" + UnitsPositions.Count.ToString();
                        UnitsPositions.Add(unit, cell);
                        unit.transform.position = Grid.PointToCoords(cell);
                        unit.transform.SetParent(transform);
                        //
                        UnitPlaced(new PlaceResult(unit, cell));
                    }
                }
            }
        }
        private void SetUnitToDisplace(GameObject unit)
        {
            CurrentUnitToMove = unit;
        }
        private void MoveUnit(Vector3 position)
        {
            RaycastHit Hit;
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out Hit))
            {
                Vector3 CurrentUnitPosition = CurrentUnitToMove.transform.position;
                SortedList<int, int> Path = PathFinder.FindPath(Grid, Grid.GetClosestPointToPosition(CurrentUnitPosition), Grid.GetClosestPointToPosition(Hit.point));
                if(Path != null)
                {
                    StartCoroutine(Move(CurrentUnitToMove, PathFinder.RestorePath(Grid, Path)));
                }
            }
        }
        private IEnumerator Move(GameObject unit, List<Vector3> moves)
        {
            foreach (Vector3 move in moves)
            {
                unit.transform.Translate(move - unit.transform.position);
                yield return new WaitForSeconds(0.5f);
            }
            MoveCompleted(new MovementResult(unit, Grid.GetClosestPointToPosition(moves[0]), Grid.GetClosestPointToPosition(moves[moves.Count - 1])));
        }

        private void UpdateUnitPosition(MovementResult result)
        {
            UnitsPositions[result.UnitDisplaced] = result.DestinationPoint;
        }

        private void FightStageUpdated(FightProps props)
        {
            if(props.Stage == FightStage.IN_PROGRESS)
            {
                OIMField.ClickDetector.OnClicked -= PlaceUnit;
                OIMField.ClickDetector.OnClicked += MoveUnit;
            }
        }
        //
        private void SavePositionsUponNewTurnStarted(FightSavedCondition condition)
        {
            condition.UnitsPositions = JsonConvert.SerializeObject(UnitsPositions);
            //
            PositionsUpdated(condition);
        }
    }
}