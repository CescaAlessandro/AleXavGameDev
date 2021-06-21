using UnityEngine;

public class SetupManager : MonoBehaviour
{
    public AudioManager am;
    public SaveManager sm;
    public MenuManager mm;
    public CanvasesBehaviour cb;

    public static SetupManager setup;

    void Start()
    {
        setup = this;
        Setup();
    }

    public static SetupManager Instance()
    {
        return setup;
    }

    public void Setup() 
    {
        cb.Setup();
        am.Setup();
        mm.Setup();
        sm.Setup();
    }
}
