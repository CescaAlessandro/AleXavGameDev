using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersBehaviour : MonoBehaviour
{
    public static ManagersBehaviour sm;

    public static ManagersBehaviour Instance()
    {
        return sm;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        if (sm == null)
        {
            sm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
