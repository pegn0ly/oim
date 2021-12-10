using UnityEngine;

namespace OIMFight
{
    // форма информации о ходе, для передачи в другие классы
    public struct TurnProps
    {
        public int FightID;
        public int Turn;

        public TurnProps(int fight_id, int turn)
        {
            FightID = fight_id;
            Turn = turn;
        }
    }

    // класс, отвечающий за смену порядка ходов в бою.

    // Поля:
    // - Fight - бой, который содержит данный компонент
    // - CurrentTurn - номер текущего хода
    
    // Ивенты:
    // - TurnStarted - вызывается при начале нового хода, передается информация о ходе

    public class TurnSequencer : MonoBehaviour
    {
        private Fight Fight;
        private int CurrentTurn = 0;

        public delegate void TurnStartedDelegate(TurnProps turn_info);
        public static event TurnStartedDelegate TurnStarted;

        private void Awake() 
        {
            Fight = (Fight)gameObject.GetComponent(typeof(Fight));
            //
            FightSequencer.OnFightStageGhanged += FightStageChanged;
            UnitDisplacer.MoveCompleted += PreviousTurnCompleted;
        }
           
        private void StartNewTurn()
        {
            CurrentTurn++;
            TurnProps TurnInfo = new TurnProps(Fight.ID, CurrentTurn);
            TurnStarted(TurnInfo);
        }

        private void PreviousTurnCompleted(MovementResult result)
        {
            StartNewTurn();
        }

        private void FightStageChanged(FightProps props)
        {
            if(props.Stage == FightStage.IN_PROGRESS)
            {
                StartNewTurn();
            }
        }
    }
}
