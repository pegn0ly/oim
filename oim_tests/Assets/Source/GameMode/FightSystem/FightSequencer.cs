using UnityEngine;

namespace OIMFight
{
    public enum FightStage
    {
        UNDEFINED = 0,
        PREPARE = 1,
        IN_PROGRESS = 2,
        PAUSED = 3,
        COMPLETED = 4
    }

    // класс, отвечающий за смену фаз боя
    // Поля:
    // - Fight - ссылка на бой, частью которого является данный класс
    // - Stage - текущая фаза боя
    // - bCanStart - готов ли бой стартовать(в данный момент используется для проверки, выставлен ли хоть 1 юнит на поле, хочу поменять)

    // Ивенты:
    // - OnFightStageGhanged - вызывается при смене фазы боя, передается фаза, в которую перешел бой
    public class FightSequencer : MonoBehaviour
    {
        private Fight Fight;
        private FightStage Stage;
        private bool bCanStart = false;
        
        public delegate void OnFightStageGhangedDelegate(FightProps props);
        public static event OnFightStageGhangedDelegate OnFightStageGhanged;

        private void Awake() 
        {
            Fight = (Fight)gameObject.GetComponent(typeof(Fight));
            //
            Fight.FightReadyToStart += StartPreparation;
            UnitDisplacer.UnitPlaced += FightCanStart;
            OnFightStageGhanged += FightStageChangeAnnounce;
        }
    
        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.F) && bCanStart)
            {
                SetFightStage(FightStage.IN_PROGRESS);
            }
        }

        public FightStage GetCurrentStage()
        {
            return Stage;
        }

        private void SetFightStage(FightStage new_stage)
        {
            if(new_stage != Stage)
            {
                Stage = new_stage;
                OnFightStageGhanged(new FightProps(Fight.ID, new_stage));
            }
        }

        private void FightStageChangeAnnounce(FightProps props)
        {
            Debug.Log("Fight " + props.Id + " stage changed to " + props.Stage.ToString());
        }

        /////////////////////////////////////////////////////////
        //
        private void StartPreparation(int fight_id)
        {
            Fight.FightReadyToStart -= StartPreparation;
            SetFightStage(FightStage.PREPARE);
        }

        // при постановке хотя бы одного юнита на поле бой можно начинать
        private void FightCanStart(PlaceResult result)
        {
            if(Stage == FightStage.PREPARE && !bCanStart)
            {
                UnitDisplacer.UnitPlaced -= FightCanStart;
                bCanStart = true;
            }
        }
    }
}