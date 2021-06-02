using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private bool firstUpdate = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUpdate)
        {
            MapUtility.Pins = new List<Pin>();

            var lowPins = GetAllObjectsOnlyInScene().Where(x => x.name.Contains("LPin")).ToList();
            var upperPins = GetAllObjectsOnlyInScene().Where(x => x.name.Contains("UPin")).ToList();

            foreach (var pin in lowPins)
            {
                var pinInstance = new Pin()
                {
                    IsConnected = false,
                    Type = PinType.Lower,
                    Instance = pin
                };

                MapUtility.Pins.Add(pinInstance);
            }

            foreach (var pin in upperPins)
            {
                var pinInstance = new Pin()
                {
                    IsConnected = false,
                    Type = PinType.Upper,
                    Instance = pin
                };

                MapUtility.Pins.Add(pinInstance);
            }

            firstUpdate = false;
        }
    }

    List<GameObject> GetAllObjectsOnlyInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }
}
