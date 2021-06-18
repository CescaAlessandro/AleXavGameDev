using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class ChipController : MonoBehaviour
{
    private GameObject pickedUpObject;

    public float gridSize = 1f;
    public GameObject cablePrefab;
    public Grid grid;
    private int cableIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (!MapUtility.GamePaused)
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
                //� presente pi� di un cavo sulla mappa
                if (currentPosition.x != transform.position.x || currentPosition.z != transform.position.z)
                {
                    if (MapUtility.IsChipWiring)//&& MapUtility.Cables.Count >= 2)
                    {
                        if (!MapUtility.IsPositionNearPin(gameObject.transform.position).Item1)
                            MapUtility.PreventReattaching = false;

                        var finalPosition = MapUtility.GetBestFinalLocationForMovement(transform.position, currentPosition);
                        transform.position = finalPosition;

                        var cable = MapUtility.Cables.First(cableA => cableA.IsConnectedToCip);
                        cable.Instance.transform.position = finalPosition;
                    }
                    else
                    {
                        //durante il drag, se Chip � vicino ad un pin (e non sta collegando) viene automaticamente attaccato a quest'ultimo
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
                }
                //rilascio del mouse/pressione touch
                if (Input.GetMouseButtonUp(0))
                {
                    pickedUpObject = null;
                    if (MapUtility.IsPositionNearPin(gameObject.transform.position).Item1)
                    {
                        //Chip vicino ad un pin
                        var pin = MapUtility.IsPositionNearPin(gameObject.transform.position).Item2;
                        if (MapUtility.IsChipWiring && pin.Type.Equals(PinType.Lower) && !pin.IsConnected && !MapUtility.PreventReattaching)
                        {
                            //Il pin vicino a Chip era della fila inferiore e Chip stava trasportando un cavo -> collego il cavo al pin
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
                        else if (MapUtility.IsChipWiring && pin.Type.Equals(PinType.Upper) && pin.IsConnected) //distruggi cavo
                        {
                            //Il pin vicino a Chip era della fila superiore, Chip sta trasportando un cavo, ed il cavo e' collegato proprio a questo pin -> scollego il cavo
                            MapUtility.setCollisionMap(gameObject.transform.position.x, gameObject.transform.position.z, CollisionEntity.getNoCollisionEntity());
                            gameObject.transform.position = new Vector3(pin.AttachmentPoint.Item1.x, 0, pin.AttachmentPoint.Item1.z + 150);


                            MapUtility.SetWiring(false);
                            pin.IsConnected = false;

                            UnityEngine.Object.Destroy(pin.CableConnected.Instance);
                            MapUtility.Cables.Remove(pin.CableConnected);
                            pin.CableConnected = null;

                            AudioManager.Instance().PlayAttachDetach();
                        }
                        else if (!MapUtility.IsChipWiring && pin.Type.Equals(PinType.Upper) && !pin.IsConnected)
                        {
                            //Il pin vicino a Chip era della fila superiore, Chip non sta trasportando un cavo, ed il pin non ha un cavo collegato -> creo un nuovo cavo che Chip trasportera'
                            gameObject.transform.position = new Vector3(pin.AttachmentPoint.Item1.x, 0, pin.AttachmentPoint.Item1.z);
                            cablePrefab.transform.position = pin.AttachmentPoint.Item1;
                            var prefabInstance = Instantiate(cablePrefab);
                            prefabInstance.GetComponent<Renderer>().material.color = pin.Instance.GetComponent<Renderer>().material.color;
                            var newCable = new Cable()
                            {
                                Instance = prefabInstance,
                                IsConnectedToCip = true,
                                index = pin.Index
                            };
                            //cableIndex++;
                            MapUtility.Cables.Add(newCable);

                            MapUtility.SetWiring(true);
                            pin.IsConnected = true;
                            pin.CableConnected = newCable;
                            TrailManager.Instance().addPoint(cablePrefab.transform.position);
                            MapUtility.setCollisionMap(cablePrefab.transform.position.x, cablePrefab.transform.position.z, CollisionEntity.getFullCollisionEntity());
                            AudioManager.Instance().PlayAttachDetach();
                        }
                        else if (!MapUtility.IsChipWiring && pin.Type.Equals(PinType.Lower) && pin.IsConnected) //stacca cavo
                        {
                            //Il pin vicino a Chip era della fila inferiore, Chip non sta trasportando un cavo, ed il pin ha un cavo collegato -> stacco il cavo dal pin e lo faccio trasportare da Chip
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
                    else if (MapUtility.IsChipWiring && MapUtility.isCollisionOnPointHole(transform.position))
                    {
                        //rilascio avvenuto su un buco mentre Chip stava trasportando un cavo
                        //aquisisco le istanze del buco, del cavo trasportato da chip
                        var chipCable = MapUtility.Cables.First(cableA => cableA.IsConnectedToCip);
                        var hole = ((Hole)MapUtility.getCollisionMap(transform.position.x, transform.position.z));
                        var holeConnected = hole.IsConnected;
                        if (holeConnected)
                        {
                            //Se il buco ha un cavo collegato significa che sono ritornato sul buco trasportando il cavo uscente da esso -> scollego/distruggo il cavo
                            hole.IsConnected = false;
                            hole.CableConnected = null;
                            hole.Exiting(directions.Top);
                            MapUtility.SetWiring(false);

                            UnityEngine.Object.Destroy(chipCable.Instance);
                            MapUtility.Cables.Remove(chipCable);

                            AudioManager.Instance().PlayAttachDetach();
                        }
                        else
                        {
                            //Il buco non ha un cavo collegato -> collego il cavo che sto trasportando al buco
                            chipCable.IsConnectedToCip = false;
                            MapUtility.SetWiring(false);
                            hole.IsConnected = true;
                            hole.CableConnected = chipCable;
                        }
                    }
                    else if (!MapUtility.IsChipWiring && MapUtility.isCollisionOnPointHole(transform.position))
                    {
                        //Chip non trasporta un cavo e rilascio la pressione su un buco
                        //Acquisisco l'istanza del buco su cui Chip si e' fermato 
                        Hole newHole = (Hole)MapUtility.getCollisionMap(transform.position.x, transform.position.z);
                        if (!newHole.IsConnected)
                        {
                            Hole connectedHole;

                            try
                            {
                                connectedHole = MapUtility.Holes.First(holeA => holeA.IsConnected == true);
                            }
                            catch (InvalidOperationException)
                            {
                                connectedHole = null;
                            }
                            if (connectedHole != null)
                            {
                                //Controllo se e' presente un buco che ha un cavo collegato -> creo un nuovo cavo e lo collego a Chip
                                cablePrefab.transform.position = transform.position;
                                var prefabInstance = Instantiate(cablePrefab);
                                prefabInstance.GetComponent<Renderer>().material.color = connectedHole.CableConnected.Instance.GetComponent<Renderer>().material.color;
                                var newCable = new Cable()
                                {
                                    Instance = prefabInstance,
                                    IsConnectedToCip = true,
                                    index = connectedHole.CableConnected.index
                                };

                                MapUtility.Cables.Add(newCable);

                                MapUtility.SetWiring(true);
                                newHole.IsConnected = true;
                                newHole.CableConnected = newCable;
                                newHole.cableCreatedOnHole();
                                TrailManager.Instance().addPoint(cablePrefab.transform.position);
                                AudioManager.Instance().PlayAttachDetach();
                            }
                        }
                        else
                        {
                            List<Hole> connectedHoles = MapUtility.Holes.Where(holeA => holeA.IsConnected == true).ToList();
                            if (connectedHoles.Count == 1)
                            {
                                //Il buco su cui Chip si e' fermato e' l'unico collegato -> scollego il cavo e Chip inizia a trasportarlo
                                MapUtility.SetWiring(true);
                                connectedHoles.ElementAt(0).IsConnected = false;
                                connectedHoles.ElementAt(0).CableConnected.IsConnectedToCip = true;
                                connectedHoles.ElementAt(0).CableConnected = null;
                            }
                            else if(connectedHoles.ElementAt(0).CableConnected.index != connectedHoles.ElementAt(1).CableConnected.index)
                            {
                                //il buco su cui Chip si e' fermato non e' l'unico ma ci sono cavi diversi collegati ai due buchi -> scollego il cavo
                                MapUtility.SetWiring(true);
                                newHole.IsConnected = false;
                                newHole.CableConnected.IsConnectedToCip = true;
                                newHole.CableConnected = null;
                            }
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
}
