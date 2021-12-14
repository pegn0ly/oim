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

    [SerializeField]
    private Text UnitDisplayer;

    private void Awake() 
    {   
        FightRegistrator.FightRegistered += DisplayFightID;
        FightSequencer.OnFightStageGhanged += DisplayFightStage;
        FightRegistrator.TurnRegistered += DisplayeCurrentTurn;
        UnitSequencer.UnitSetAsCurrent += DisplayCurrentUnitName;
    }

    private void DisplayFightID(FightProps props)
    {
        IDDisplayer.text = "FightID: " + props.Id.ToString();
    }

    private void DisplayFightStage(FightProps props)
    {
        switch(props.Stage)
        {
            case FightStage.PREPARE:
            {
                StageDisplayer.text = "Фаза подготовки - клики по полю размещают юнитов. Нажмите F, чтобы начать бой(хотя бы 1 юнит должен быть выставлен)";
                break;
            }
            case FightStage.IN_PROGRESS:
            {
                StageDisplayer.text = "Активная фаза боя - можно перемещать юнитов по полю";
                break;
            }
            default:
                break;
        }
    }

    private void DisplayeCurrentTurn(FightSavedCondition condition)
    {
        TurnDisplayer.text = "Current turn: " + condition.Turn.ToString();
    }

    private void DisplayCurrentUnitName(GameObject unit)
    {
        UnitDisplayer.text = unit.name + " is moving ";
    }
}
