
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Linq;

[RequireComponent(typeof(MeshCollider))]

public class ChipController : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public float gridSize = 1f;
    public GameObject cablePrefab;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && MapUtility.IsChipNearPin(gameObject.transform.position).Item1)
        {
            var pin = MapUtility.IsChipNearPin(gameObject.transform.position).Item2;

            if (MapUtility.IsChipWiring && pin.Type.Equals(PinType.Lower) && !pin.IsConnected)
            {
                gameObject.transform.position = pin.Instance.transform.GetChild(0).position;

                var cable = MapUtility.Cables.First(cable => cable.IsConnectedToCip);
                cable.Instance.transform.position = pin.Instance.transform.position;
                cable.Instance.transform.rotation = pin.Instance.transform.GetChild(0).rotation;
                cable.IsConnectedToCip = false;

                MapUtility.SetWiring(false);
            }

            if (!MapUtility.IsChipWiring && pin.Type.Equals(PinType.Upper) && !pin.IsConnected)
            {
                gameObject.transform.position = pin.Instance.transform.GetChild(0).position;
                cablePrefab.transform.position = pin.Instance.transform.GetChild(0).position;

                var prefabInstance = Instantiate(cablePrefab);
                prefabInstance.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();

                var newCable = new Cable()
                {
                    Instance = prefabInstance,
                    IsConnectedToCip = true,
                };

                MapUtility.Cables.Add(newCable);

                MapUtility.SetWiring(true);
                pin.IsConnected = true;
            }
        }
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        curPosition.x = ((int)(curPosition.x / gridSize)) * gridSize;
        curPosition.z = ((int)(curPosition.z / gridSize)) * gridSize;

        if (curPosition.x < -40)
            curPosition.x = -40;

        if (curPosition.x > 40)
            curPosition.x = 40;

        if (curPosition.z < -50)
            curPosition.z = -50;

        if (curPosition.z > 50)
            curPosition.z = 50;

        //limita gli spostamenti a scacchiera
        if(!(curPosition.x != transform.position.x && curPosition.z != transform.position.z))
        {
            //eventuali collisioni vanno controllate solo se 
            //Cip sta maneggiando un cavo 
            //AND
            //è presente più di un cavo sulla mappa
            if (MapUtility.IsChipWiring && MapUtility.Cables.Count >= 2)
            {
                var startPosition = new Tuple<float, float>(transform.position.x, transform.position.z);
                var finishPosition = new Tuple<float, float>(curPosition.x, curPosition.z);

                if (!MapUtility.LookForCollision(startPosition, finishPosition))
                    transform.position = curPosition;
            }
            else
                transform.position = curPosition;
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            Debug.Log("double click");
        }
    }
}
