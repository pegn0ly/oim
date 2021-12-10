using OIMField;
using OIMFight;

public class OIMGameMode : GameModeBase
{
    private Field FieldDefinition;
    private Fight FightDefinition;

    private FightRegistrator Registrator;
    private FieldGenerator Generator;

    private void Awake()
    {
        Registrator = (FightRegistrator)gameObject.AddComponent(typeof(FightRegistrator));
        Generator = (FieldGenerator)gameObject.AddComponent(typeof(FieldGenerator));
        //
        FightRegistrator.FightRegistered += SetupFight;
        FieldGenerator.FieldGenerated += SetupField;
        //
        transform.SetParent(transform.parent);
    }

    void Start()
    {
        Registrator.RegisterNewFight();
        Generator.StartGeneration();
    }

    private void SetupFight(FightProps props)
    {
        FightDefinition = (Fight)gameObject.AddComponent(typeof(Fight));
        FightDefinition.InitFight(props);
    }

    private void SetupField(GridProps props)
    {
        FieldDefinition = (Field)gameObject.AddComponent(typeof(Field));
        FieldDefinition.InitField(props);
    }
}