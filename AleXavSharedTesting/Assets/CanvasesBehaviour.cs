using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasesBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    public static CanvasesBehaviour sm;

    public static CanvasesBehaviour Instance()
    {
        return sm;
    }

    public void Setup()
    {
        DontDestroyOnLoad(this);

        if(sm == null)
        {
            sm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
