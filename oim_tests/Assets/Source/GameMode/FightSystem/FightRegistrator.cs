using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using UnityEngine;

using OIMWeb;

// FightRegistrator предназначен для передачи/получения информации о бое в бэкенд

namespace OIMFight
{   
    // форма общей информации о бое
    public struct FightProps
    {
        public int Id;
        public FightStage Stage;

        public FightProps(int id, FightStage stage)
        {
            Id = id;
            Stage = stage;
        }
    }

    // форма информации о сохраненном состоянии боя
    public struct FightSavedCondition
    {
        public int FightID;
        public int Turn;
        public string UnitsPositions;

        public FightSavedCondition(int fight_id, int turn, string positions)
        {
            FightID = fight_id;
            Turn = turn;
            UnitsPositions = positions;
        }
    }

    // регистрирует бой в бэкенде
    public class FightRegistrator : MonoBehaviour
    {
        // делегат, вызываемый при успешной регистрации, передает параметры зарегистрированного боя
        public delegate void OnFightRegistered(FightProps props);
        public static event OnFightRegistered FightRegistered;

        public delegate void OnTurnRegistered(FightSavedCondition props);
        public static event OnTurnRegistered TurnRegistered;

        private void Awake() 
        {
            FightSequencer.OnFightStageGhanged += UpdateFightInfo;
            TurnSequencer.TurnStarted += RegisterNewTurn;

            UnitDisplacer.PositionsUpdated += UpdateFightCondition;
        }
        ///////////////////////////////////////////////////////////////////////////
        // процесс регистрации нового боя
        public void RegisterNewFight()
        {
            // получить информацию о последнем зарегистрированном бое, чтобы определить id для регистрации текущего
            ContentRequest Request = new ContentRequest("fight/last");
            Game.Instance.DefaultWebManager.Get(Request, SetupNewFight);
        }

        private void SetupNewFight(object obj)
        {
            FightProps LastFight = JsonUtility.FromJson<FightProps>(obj.ToString());

            string NewFightId = Convert.ToString(LastFight.Id + 1);
            Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add("Id", NewFightId);
            Data.Add("Stage", FightStage.UNDEFINED.ToString());
            
            ContentRequest Request = new ContentRequest("fight/new/", Data);
            Game.Instance.DefaultWebManager.Post(Request, NewFightRegistered);
        }

        private void NewFightRegistered(object obj)
        {
            Debug.Log("New fight registered with id " + JsonUtility.FromJson<FightProps>(obj.ToString()).Id);
            FightRegistered(JsonUtility.FromJson<FightProps>(obj.ToString()));
        }

        //////////////////////////////////////////////////////////////////////////////
        // обновление информации о бое
        private void UpdateFightInfo(FightProps fight_to_update)
        {
            Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add("Id", fight_to_update.Id.ToString());
            Data.Add("Stage", fight_to_update.Stage.ToString());
            
            ContentRequest Request = new ContentRequest("fight/update/", Data);
            Game.Instance.DefaultWebManager.Post(Request, null);
        }

        ///////////////////////////////////////////////////////////////////////
        // сохранение информации о ходе
        private void RegisterNewTurn(TurnProps turn_info)
        {
            Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add("FightID", turn_info.FightID.ToString());
            Data.Add("Turn", turn_info.Turn.ToString());
            //
            ContentRequest Request = new ContentRequest("fight/turn/new/", Data);
            Game.Instance.DefaultWebManager.Post(Request, NewTurnRegistered);
        }
        private void NewTurnRegistered(object obj)
        {
            FightSavedCondition RegisteredTurn = JsonUtility.FromJson<FightSavedCondition>(obj.ToString());
            TurnRegistered(RegisteredTurn);
        }
        private void UpdateFightCondition(FightSavedCondition condition)
        {
            Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add("FightID", condition.FightID.ToString());
            Data.Add("Turn", condition.Turn.ToString());
            Data.Add("UnitsPositions", condition.UnitsPositions);
            //
            ContentRequest Request = new ContentRequest("fight/turn/update/", Data);
            Game.Instance.DefaultWebManager.Post(Request, null);
        }
    }
}
