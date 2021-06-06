using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Linq;

public class ChipController : MonoBehaviour
{
    private GameObject pickedUpObject;

    public float gridSize = 1f;
    public GameObject cablePrefab;
    public Grid grid;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUpObject != null)
        {
            Vector3 MouseworldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 65.0f));
            var currentPosition = grid.GetCellCenterWorld(grid.WorldToCell(new Vector3(MouseworldPoint.x, pickedUpObject.transform.position.y, MouseworldPoint.z)));           

            if (currentPosition.x < -400)
                currentPosition.x = -400;
            if (currentPosition.x > 400)
                currentPosition.x = 400;

            if (currentPosition.z < -500)
                currentPosition.z = -500;

            if (currentPosition.z > 500)
                currentPosition.z = 500;

            //eventuali collisioni vanno controllate solo se 
            //Cip sta maneggiando un cavo 
            //AND
            //è presente più di un cavo sulla mappa
            if (MapUtility.IsChipWiring && MapUtility.Cables.Count >= 2)
            {
                var startPosition = new Tuple<float, float>(transform.position.x, transform.position.z);
                var finishPosition = new Tuple<float, float>(currentPosition.x, transform.position.z);

                if (!MapUtility.LookForCollision(startPosition, finishPosition))
                {
                    transform.position = new Vector3(currentPosition.x, transform.position.y, transform.position.z);
                    TrailManager.Instance().updateCable();
                    startPosition = new Tuple<float, float>(transform.position.x, transform.position.z);
                    finishPosition = new Tuple<float, float>(transform.position.x, currentPosition.z);
                    if (!MapUtility.LookForCollision(startPosition, finishPosition))
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, currentPosition.z);
                        TrailManager.Instance().updateCable();
                    }
                }
            }
            else
            {
                transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.z);
                TrailManager.Instance().updateCable();
            }

            if (Input.GetMouseButtonUp(0))
                pickedUpObject = null;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Ray m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit m_hit;
                bool result = Physics.Raycast(m_ray, out m_hit);
                if (result)
                    pickedUpObject = m_hit.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonDown(1) && MapUtility.IsChipNearPin(gameObject.transform.position).Item1)
        {
            var pin = MapUtility.IsChipNearPin(gameObject.transform.position).Item2;

            if (MapUtility.IsChipWiring && pin.Type.Equals(PinType.Lower) && !pin.IsConnected)
            {
                gameObject.transform.position = pin.AttachmentPoint.Item1;

                var cable = MapUtility.Cables.First(cable => cable.IsConnectedToCip);
                cable.Instance.transform.position = pin.AttachmentPoint.Item1 + new Vector3(0, 10, 0) ;
                cable.Instance.transform.rotation = pin.AttachmentPoint.Item2;
                cable.IsConnectedToCip = false;
                
                MapUtility.SetWiring(false);
                pin.IsConnected = true;
                pin.CableConnected = cable;

                GameManager.Instance().CheckForPossibleDepletion(pin);
            }

            if (!MapUtility.IsChipWiring && pin.Type.Equals(PinType.Upper) && !pin.IsConnected)
            {
                gameObject.transform.position = pin.AttachmentPoint.Item1;
                cablePrefab.transform.position = pin.AttachmentPoint.Item1;

                var prefabInstance = Instantiate(cablePrefab);
                prefabInstance.GetComponent<Renderer>().material.color = pin.Instance.GetComponent<Renderer>().material.color;

                var newCable = new Cable()
                {
                    Instance = prefabInstance,
                    IsConnectedToCip = true,
                };

                MapUtility.Cables.Add(newCable);

                MapUtility.SetWiring(true);
                pin.IsConnected = true;
                pin.CableConnected = newCable;
            }
        }
    }
}
