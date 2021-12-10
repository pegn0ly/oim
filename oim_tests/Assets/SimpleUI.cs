using UnityEngine;
using UnityEngine.UI;

using OIMFight;

public class SimpleUI : MonoBehaviour
{
    [SerializeField]
    private Text IDDisplayer;
    
    [SerializeField]
    private Text StageDisplayer;

    [SerializeField]
    private Text TurnDisplayer;

    private void Awake() 
    {   
        FightRegistrator.FightRegistered += DisplayFightID;
        FightSequencer.OnFightStageGhanged += DisplayFightStage;
        FightRegistrator.TurnRegistered += DisplayeCurrentTurn;
    }

    private void DisplayFightID(FightProps props)
    {
        IDDisplayer.text = "FightID: " + props.Id.ToString();
    }

    private void DisplayFightStage(FightProps props)
    {
        StageDisplayer.text = "Current stage: " + props.Stage.ToString();
    }

    private void DisplayeCurrentTurn(FightSavedCondition condition)
    {
        TurnDisplayer.text = "Current turn: " + condition.Turn.ToString();
    }
}
