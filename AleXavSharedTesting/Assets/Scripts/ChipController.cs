using UnityEngine;
using System;
using System.Linq;

public class ChipController : MonoBehaviour
{
    private GameObject pickedUpObject;

    public float gridSize = 1f;
    public GameObject cablePrefab;
    public Grid grid;
    private Vector3 lastMovementStart = new Vector3(-1, -1, -1);

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
            if (MapUtility.IsChipWiring)//&& MapUtility.Cables.Count >= 2)
            {
                if (!MapUtility.IsPositionNearPin(gameObject.transform.position).Item1)
                    MapUtility.PreventReattaching = false;

                var startPosition = new Tuple<float, float>(transform.position.x, transform.position.z);
                var finishPosition = new Tuple<float, float>(currentPosition.x, currentPosition.z);

                if (!TrailManager.Instance().isLastMove(new Vector3(finishPosition.Item1,0,finishPosition.Item2)))
                {
                    if (!MapUtility.LookForCollision(startPosition, finishPosition))
                    {
                        if (startPosition.Item1 != finishPosition.Item1 && startPosition.Item2 != finishPosition.Item2)
                        {
                            transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.z);
                            var cable = MapUtility.Cables.First(cableA => cableA.IsConnectedToCip);
                            cable.Instance.transform.position = transform.position;
                            TrailManager.Instance().cableUpdate();
                            MapUtility.setCollisionMap(currentPosition.x, currentPosition.z, true);
                            MapUtility.setCollisionMap(currentPosition.x+100, currentPosition.z, true);
                        }
                        else
                        {
                            transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.z);
                            var cable = MapUtility.Cables.First(cableA => cableA.IsConnectedToCip);
                            cable.Instance.transform.position = transform.position;
                            TrailManager.Instance().cableUpdate();
                            MapUtility.setCollisionMap(currentPosition.x, currentPosition.z, true);
                        }
                    }
                    else
                    {
                        //Debug.Log("Collision found.");
                    }
                }
                else
                {
                    MapUtility.setCollisionMap(transform.position.x, transform.position.z, false);
                    transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.z);
                    var cable = MapUtility.Cables.First(cableA => cableA.IsConnectedToCip);
                    cable.Instance.transform.position = transform.position;
                    TrailManager.Instance().cableUpdate();
                }
            }
            else
            {
                //durante il drag, se Chip è vicino ad un pin (e non sta collegando) viene automaticamente attaccato a quest'ultimo
                //(drop on mobile)
                if (MapUtility.IsPositionNearPin(currentPosition).Item1)
                {
                    var pin = MapUtility.IsPositionNearPin(currentPosition).Item2;
                    gameObject.transform.position = new Vector3(pin.AttachmentPoint.Item1.x, 0, pin.AttachmentPoint.Item1.z);
                }
                else
                {
                    transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.z);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                pickedUpObject = null;
                if (MapUtility.IsPositionNearPin(gameObject.transform.position).Item1)
                {
                    var pin = MapUtility.IsPositionNearPin(gameObject.transform.position).Item2;

                    if (MapUtility.IsChipWiring && pin.Type.Equals(PinType.Lower) && !pin.IsConnected && !MapUtility.PreventReattaching)
                    {
                        gameObject.transform.position = new Vector3(pin.AttachmentPoint.Item1.x, 0, pin.AttachmentPoint.Item1.z);
                        var cable = MapUtility.Cables.First(cableA => cableA.IsConnectedToCip);
                        cable.Instance.transform.position = pin.AttachmentPoint.Item1 + new Vector3(0, 0, 0);
                        cable.Instance.transform.rotation = pin.AttachmentPoint.Item2;
                        cable.IsConnectedToCip = false;

                        MapUtility.SetWiring(false);
                        pin.IsConnected = true;
                        pin.CableConnected = cable;

                        AudioManager.Instance().PlayAttachDetach();
                    }
                    else if(MapUtility.IsChipWiring && pin.Type.Equals(PinType.Upper) && pin.IsConnected) //distruggi cavo
                    {
                        //Chip si porta ad una casella rispettivamente sotto il pin 
                        gameObject.transform.position = new Vector3(pin.AttachmentPoint.Item1.x, 0, pin.AttachmentPoint.Item1.z + 150);

                        MapUtility.setCollisionMap(pin.CableConnected.Instance.transform.position.x, pin.CableConnected.Instance.transform.position.z, false);
                        MapUtility.SetWiring(false);
                        pin.IsConnected = false;

                        UnityEngine.Object.Destroy(pin.CableConnected.Instance);
                        MapUtility.Cables.Remove(pin.CableConnected);
                        pin.CableConnected = null;

                        AudioManager.Instance().PlayAttachDetach();
                    }
                    else if (!MapUtility.IsChipWiring && pin.Type.Equals(PinType.Upper) && !pin.IsConnected)
                    {
                        gameObject.transform.position = new Vector3(pin.AttachmentPoint.Item1.x, 0, pin.AttachmentPoint.Item1.z);
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
                        TrailManager.Instance().cableUpdate();

                        AudioManager.Instance().PlayAttachDetach();
                    }
                    else if (!MapUtility.IsChipWiring && pin.Type.Equals(PinType.Lower) && pin.IsConnected) //stacca cavo
                    {
                        //impedisco a Chip di attaccare immediatamente il cavo appena scollegato
                        MapUtility.PreventReattaching = true;
                        MapUtility.SetWiring(true);

                        pin.IsConnected = false;
                        pin.CableConnected.IsConnectedToCip = true;
                        TrailManager.Instance().UpdateCablePointsOnDetach();
                        pin.CableConnected.Instance.transform.position = gameObject.transform.position;

                        GameManager.Instance().CheckForPossibleDepletionPauses(pin);

                        AudioManager.Instance().PlayAttachDetach();
                    }
                }                         
            }   
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
    }
}
