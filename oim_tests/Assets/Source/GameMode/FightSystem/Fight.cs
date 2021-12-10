using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OIMWeb;

namespace OIMFight
{
    // основной класс, представляющий бой.
    public class Fight : MonoBehaviour
    {
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        private FightSequencer FightSequencer;
        private TurnSequencer TurnSequencer;
        private UnitSequencer UnitSequencer;
        private UnitDisplacer UnitDisplacer;

        /////////////////////////////////////////////////////////
        public delegate void FightReadyToStartDelegate(int id);
        public static event FightReadyToStartDelegate FightReadyToStart;

        private void Awake() 
        {
            //transform.SetParent(transform);
        }
        /////////////////////////////////////////////////////////
        private void Update()
        {
            if(ID != 0 && (FightSequencer.GetCurrentStage() == FightStage.UNDEFINED))
            {
                FightReadyToStart(ID);
            }
        }

        /////////////////////////////////////////////////////////
        public void InitFight(FightProps props)
        {
            _ID = props.Id;

            FightSequencer = (FightSequencer)gameObject.AddComponent(typeof(FightSequencer));
            TurnSequencer = (TurnSequencer)gameObject.AddComponent(typeof(TurnSequencer));
            UnitSequencer = (UnitSequencer)gameObject.AddComponent(typeof(UnitSequencer));
            UnitDisplacer = (UnitDisplacer)gameObject.AddComponent(typeof(UnitDisplacer));
        }
    }
}