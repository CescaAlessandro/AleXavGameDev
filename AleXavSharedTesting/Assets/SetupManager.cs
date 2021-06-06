using UnityEngine;

public class SetupManager : MonoBehaviour
{
    public AudioManager am;
    public SaveManager sm;
    public MenuManager mm;

    void Start()
    {
        am.Setup();
        mm.Setup();
        sm.Setup();
    }
}
