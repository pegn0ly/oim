using UnityEngine;
using OIMWeb;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameModeBase _DefaultGameMode;

    [SerializeField]
    private WebManager _DefaultWebManager;
    public WebManager DefaultWebManager
    {
        get
        {
            return _DefaultWebManager;
        }
    }

    public static Game Instance{ get; private set;}

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}