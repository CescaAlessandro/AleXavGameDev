using UnityEngine;

public class SetupManager : MonoBehaviour
{
    public AudioManager am;
    public SaveManager sm;
    public MenuManager mm;

    void Start()
    {
        DontDestroyOnLoad(this);
        am.Setup();
        mm.Setup();
        sm.Setup();
    }
}
