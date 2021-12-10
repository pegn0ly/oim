using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

namespace OIMFight
{
    // класс, отвечающий за смену порядка хода юнитов в бою.
    // Поля:
    // - MainSequence - связный список, представляющий порядок хода юнитов
    // - CurrentActiveUnit - юнит, который должен ходить в данный момент
    // Ивенты:
    // - UnitSetAsCurrent - вызывается, когда новый юнит становится текущим, передается этот юнит
    public class UnitSequencer : MonoBehaviour
    {
        private LinkedList<GameObject> MainSequence = new LinkedList<GameObject>();
        private GameObject CurrentActiveUnit;
        //
        public delegate void OnUnitSetAsCurrent(GameObject unit);
        public static event OnUnitSetAsCurrent UnitSetAsCurrent;

        // подписываемся на ивенты UnitDisplacer'а, чтобы расстановка и перемещение юнитов могли влиять на смену порядка ходов
        // подписываемся на ивент смены стадии боя, чтобы при переходе боя в стадию IN_PROGRESS, последовательность бы начала свое продвижение
        private void Awake() 
        {
            UnitDisplacer.UnitPlaced += AddUnitToSequence; 
            UnitDisplacer.MoveCompleted += MoveSequenceToNext;

            FightSequencer.OnFightStageGhanged += CheckFightInProgress;
        }

        private void AddUnitToSequence(PlaceResult place_result)
        {
            MainSequence.AddLast(place_result.UnitPlaced);
        }

        private void CheckFightInProgress(FightProps props)
        {
            if(props.Stage == FightStage.IN_PROGRESS)
            {
                FightSequencer.OnFightStageGhanged -= CheckFightInProgress;
                //
                CurrentActiveUnit = MainSequence.First.Value;
                UnitSetAsCurrent(CurrentActiveUnit);
            }
        }

        private void MoveSequenceToNext(MovementResult move_result)
        {
            if(CurrentActiveUnit == MainSequence.Last.Value)
            {
                CurrentActiveUnit = MainSequence.First.Value;
            }
            else
            {
                CurrentActiveUnit = MainSequence.Find(CurrentActiveUnit).Next.Value;
            }
            UnitSetAsCurrent(CurrentActiveUnit);
        }
    }
}