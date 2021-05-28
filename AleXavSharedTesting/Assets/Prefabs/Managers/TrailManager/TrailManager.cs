using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    private List<Cable> cables;

    public GameObject target;
    public GameObject cablePrefab;

    // Start is called before the first frame update
    void Start()
    {
        cables = new List<Cable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //creo un nuovo cavo solo se Cip non ne sta già maneggiando
            if (!MapUtility.IsCipWiring)
            {
                cablePrefab.transform.position = target.transform.position;
                cablePrefab.transform.rotation = target.transform.rotation;

                var prefabInstance = Instantiate(cablePrefab);
                prefabInstance.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();

                var newCable = new Cable()
                {
                    Instance = prefabInstance,
                    IsConnectedToCip = true,
                };

                cables.Add(newCable);
                MapUtility.SetWiring(true);
            }     
        }

        foreach(var cable in cables)
        {
            //aggiorno e salvo la posizione del cavo solo se questo è maneggiato da Cip
            if (cable.IsConnectedToCip)
            {
                cable.Instance.transform.position = target.transform.position;
                cable.Instance.transform.rotation = target.transform.rotation;
                var newPosition = new Tuple<float, float>(cable.Instance.transform.position.x, cable.Instance.transform.position.z);
                cable.AddPosition(newPosition);
            }
        }

        //eventuali collisioni vanno controllate solo se 
        //Cip sta maneggiando un cavo 
        //AND
        //è presente più di un cavo sulla mappa
        if(MapUtility.IsCipWiring && cables.Count >= 2)
        {
            for (int i = 0; i < cables.Count - 1; i++)
            {
                if (MapUtility.SharesAnyValueWith<Tuple<float, float>>(cables.ElementAt(i).WirePositions, cables.ElementAt(i + 1).WirePositions))
                    Debug.Log("Collisione rilevata.");
            }
        }


        //da modificare
        if (Input.GetKeyDown("1"))
        {
            if(cables.ElementAt(0) != null)
            {
                MapUtility.SetWiring(false);
                cables.ElementAt(0).IsConnectedToCip = false;
            }
        }
        if (Input.GetKeyDown("2"))
        {
            if (cables.ElementAt(1) != null)
            {
                MapUtility.SetWiring(false);
                cables.ElementAt(1).IsConnectedToCip = false;
            }
        }
        if (Input.GetKeyDown("3"))
        {
            if (cables.ElementAt(2) != null)
            {
                MapUtility.SetWiring(false);
                cables.ElementAt(2).IsConnectedToCip = false;
            }
        }
    }

    private class Cable
    {
        public GameObject Instance { get; set; }
        public bool IsConnectedToCip { get; set; }

        public List<Tuple<float, float>> WirePositions = new List<Tuple<float, float>>();

        public void AddPosition(Tuple<float, float> position)
        {
            if (!WirePositions.Contains(position))
            {
                WirePositions.Add(position);
            }
        }
    }
}
