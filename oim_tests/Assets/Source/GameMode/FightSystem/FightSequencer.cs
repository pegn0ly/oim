using UnityEngine;

// FightSequencer отвечает за смену фаз боя.

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

    public class FightSequencer : MonoBehaviour
    {
        // ссылка на бой, частью которого является данный класс
        private Fight Fight;
        private FightStage Stage;

        // делегаты
        public delegate void OnFightStageGhangedDelegate(FightProps props);
        public static event OnFightStageGhangedDelegate OnFightStageGhanged;

        private void Awake() 
        {
            Fight = (Fight)gameObject.GetComponent(typeof(Fight));
            //
            Fight.FightReadyToStart += StartPreparation;
            OnFightStageGhanged += FigthStageChangeAnnounce;
        }
    
        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.F) && Stage == FightStage.PREPARE)
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

        private void FigthStageChangeAnnounce(FightProps props)
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
    }
}