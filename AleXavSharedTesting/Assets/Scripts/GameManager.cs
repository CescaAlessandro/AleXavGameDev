using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private List<Flux> idleFluxes;
    private List<Flux> depletingFluxes;

    public GameObject fluxPrefab;
    //spawnPoints presi dagli attachment dello scenario, andr� tolto
    public List<GameObject> spawnPoints;
    public Sprite[] livesSprites = new Sprite[3];
    public bool CanWin;
    public int fluxesDepletedToWin;

    public SpriteRenderer livesIndicator;
    public bool preventLoosingLife;
    public bool preventFluxSpawning;
    public DudeBehaviour Dude;

    public Camera camera;

    private int maxLives = 3;
    private int lives;
    private int numberFluxesDepleteded;
    private int numberFluxesSpawned;
    private bool levelCompleted;

    public static GameManager gm;

    void Start()
    {
        camera.orthographicSize = Screen.height * 800 / 1920;
        gm = this;
        lives = maxLives;
        numberFluxesDepleteded = 0;
        levelCompleted = false;

        SaveManager.Instance().SaveLastScene(SceneManager.GetActiveScene().name);

        MapUtility.LowerPins = new List<Pin>();
        MapUtility.UpperPins = new List<Pin>();
        MapUtility.Holes = new List<Hole>();
        MapUtility.Bridges = new List<Bridge>();

        MapUtility.IsChipWiring = false;

        var lowPins = MapUtility.GetAllObjectsOnlyInScene().Where(x => x.name.Contains("LPin")).OrderBy(pin => pin.name).ToList();
        var upperPins = MapUtility.GetAllObjectsOnlyInScene().Where(x => x.name.Contains("UPin")).OrderBy(pin => pin.name).ToList();
        var holes = MapUtility.GetAllObjectsOnlyInScene().Where(x => x.name.Contains("Hole")).OrderBy(hole => hole.name).ToList();
        var bridges = MapUtility.GetAllObjectsOnlyInScene().Where(x => x.name.Contains("Bridge")).ToList();   
        MapUtility.collisionMapBaseSetup();

        //per ogni lowPin trovato, creo un'istanza di Pin
        var pinIndex = 0;
        foreach (var pin in lowPins)
        {
            var attachmentPosition = pin.transform.GetChild(0).position;
            var attachmentRotation = pin.transform.GetChild(0).rotation;
            var attachment = new Tuple<Vector3, Quaternion>(attachmentPosition, attachmentRotation);

            var pinInstance = new Pin()
            {
                IsConnected = false,
                Type = PinType.Lower,
                Instance = pin,
                AttachmentPoint = attachment,
                Index = pinIndex
            };
            MapUtility.setCollisionMap(attachmentPosition.x, attachmentPosition.z, CollisionEntity.getFullCollisionEntity());
            MapUtility.LowerPins.Add(pinInstance);
            pinIndex++;
        }

        //per ogni upperPin trovato, creo un'istanza di Pin
        pinIndex = 0;
        foreach (var pin in upperPins)
        {
            var attachmentPosition = pin.transform.GetChild(0).position;
            var attachmentRotation = pin.transform.GetChild(0).rotation;
            var attachment = new Tuple<Vector3, Quaternion>(attachmentPosition, attachmentRotation);

            var spawnPointPosition = new Vector3(attachmentPosition.x, attachmentPosition.y, attachmentPosition.z - 950);
            var fluxSpawnPoint = new Tuple<Vector3, Quaternion>(spawnPointPosition, attachmentRotation);

            var pinInstance = new Pin()
            {
                IsConnected = false,
                Type = PinType.Upper,
                Instance = pin,
                FluxSpawnPoint = fluxSpawnPoint,
                AttachmentPoint = attachment,
                Index = pinIndex
            };
            MapUtility.setCollisionMap(attachmentPosition.x, attachmentPosition.z, CollisionEntity.getFullCollisionEntity());
            MapUtility.UpperPins.Add(pinInstance);
            pinIndex++;
        }

        idleFluxes = new List<Flux>();
        depletingFluxes = new List<Flux>();


        foreach (var hole in holes)
        {
            var holeInstance = new Hole()
            {
                IsConnected = false,
                Instance = hole
            };

            MapUtility.Holes.Add(holeInstance);
            MapUtility.setCollisionMap(hole.transform.position.x, hole.transform.position.z, holeInstance);
        }
        foreach (var bridge in bridges)
        {
            var bridgeInstance = new Bridge();
            MapUtility.Bridges.Add(bridgeInstance);
            MapUtility.setCollisionMap(bridge.transform.position.x, bridge.transform.position.z, bridgeInstance);
        }
        //flag per prevenire lo spawn dei flussi, == true nei tutorial
        if (!preventFluxSpawning)
        {
            //il livello 7 prevede la possibilità di spawnare due flussi contemporaneamente su due pin diversi
            if (SceneManager.GetActiveScene().name == "Level 7")
            {
                StartCoroutine(spawnRandomFluxesForeverWithDoubleFlux());
            }
            else
            {
                StartCoroutine(spawnRandomFluxesForever());
            }
        }

        MapUtility.GamePaused = false;
    }

    void Update()
    {
        /*canWin == true se non è un tutorial, e quindi il 'level complete' è lanciato dal game manager raggiunta la condizione di vittoria */
        if (CanWin && fluxesDepletedToWin <= numberFluxesDepleteded && !levelCompleted)
        {
            levelCompleted = true;
            Dude.LevelCompletedBehaviour();
        }

        //se premo Esc e non sono attivi altri menu => vado in pausa
        if (Input.GetKeyDown(KeyCode.Escape) && !MenuManager.Instance().GetMenusStatus())
        {
            MapUtility.GamePaused = true;
            MenuManager.Instance().LoadPauseMenu();
        }

        if (!MapUtility.GamePaused)
        {
            //quando collego un lower pin, controllo se il rispettivo upper pin ha un flusso in attesa
            foreach (var lPin in MapUtility.LowerPins.Where(pin => pin.IsConnected))
                CheckForPossibleDepletion(lPin);

            //attivo il suono zap e aggiorno il timer di richiesta per ogni flusso in Idle
            foreach (var flux in idleFluxes)
            {
                AudioManager.Instance().PlayZap();

                flux.requestTimer += Time.deltaTime;

                //se il timer è sopra la soglia, si perde una vita
                if (flux.requestTimer >= 5)
                {
                    if (!preventLoosingLife)
                        LoseLives(1);
                    flux.requestTimer = 0;
                }
            }
        }
    }

    public static GameManager Instance()
    {
        return gm;
    }

    public int GetNumberFluxesDepleteded()
    {
        return this.numberFluxesDepleteded;
    }

    //logic to decide what to do on flux arrival
    private void onFluxArrival()
    {
        // TODO
    }
    //logic to decide what to do on flux depletion
    private void onFluxDepletion()
    {

    }
    public int GetCurrentLives()
    {
        return lives;
    }
    public void LoseLives(int amount)
    {
        lives -= amount;
        if (lives > 0)
        {
            livesIndicator.sprite = livesSprites[lives];
            AudioManager.Instance().PlayLoseLife();
        }
        else
        {
            livesIndicator.sprite = livesSprites[0];
            Dude.GameOverBehaviour();
        }
    }
    //spawn fluxes on random pins with a fixed delay between them
    private float fluxSpawnDelay = 15;
    private List<Pin> spawnablePins;
    IEnumerator spawnRandomFluxesForever()
    {
        spawnablePins = MapUtility.UpperPins.ToList();
        numberFluxesSpawned = 0;

        while (true)
        {
            if (!MapUtility.GamePaused)
            {
                if (numberFluxesSpawned < fluxesDepletedToWin)
                {
                    int ranInd = UnityEngine.Random.Range(0, spawnablePins.Count);
                    SpawnFluxIndex(spawnablePins.ElementAt(ranInd).Index);
                    spawnablePins.RemoveAt(ranInd);
                    //Debug.Log("Spawned at index: " + ranInd);
                    numberFluxesSpawned++;

                }
                yield return new WaitForSeconds(fluxSpawnDelay);
            }
            else
                yield return null;
        }
    }

    //random fluxes with the chance of 2 fluxes at the same time
    private int spawnDoubleFluxOnceEvery = 4;
    IEnumerator spawnRandomFluxesForeverWithDoubleFlux()
    {
        spawnablePins = MapUtility.UpperPins.ToList();
        numberFluxesSpawned = 0;

        while (true)
        {
            if (!MapUtility.GamePaused)
            {
                if (numberFluxesSpawned < fluxesDepletedToWin)
                {
                    int doubleFluxChance = UnityEngine.Random.Range(0, spawnDoubleFluxOnceEvery);
                    if (doubleFluxChance == 3)
                    {
                        int ranInd1 = UnityEngine.Random.Range(0, spawnablePins.Count);
                        int ranInd2 = ranInd1;
                        while (ranInd1 == ranInd2)
                        {
                            ranInd2 = UnityEngine.Random.Range(0, spawnablePins.Count);
                        }
                        var pin1 = spawnablePins.ElementAt(ranInd1);
                        var pin2 = spawnablePins.ElementAt(ranInd2);
                        SpawnFluxIndex(pin1.Index);
                        SpawnFluxIndex(pin2.Index);
                        spawnablePins.Remove(pin1);
                        spawnablePins.Remove(pin2);
                        numberFluxesSpawned += 2;
                    }
                    else
                    {
                        int ranInd = UnityEngine.Random.Range(0, spawnablePins.Count);
                        SpawnFluxIndex(spawnablePins.ElementAt(ranInd).Index);
                        spawnablePins.RemoveAt(ranInd);
                        numberFluxesSpawned++;
                        //Debug.Log("Spawned at index: " + ranInd);
                    }
                }
                yield return new WaitForSeconds(fluxSpawnDelay);
            }
            else
                yield return null;
        }
    }

    //spawn a flux that will arrive on the pin corresponding to the index passed
    public Flux SpawnFluxIndex(int index)
    {
        var pin = MapUtility.UpperPins.FirstOrDefault(pinA => pinA.Index == index);

        GameObject inst = GameObject.Instantiate(fluxPrefab, pin.FluxSpawnPoint.Item1, pin.FluxSpawnPoint.Item2, this.transform);
        inst.GetComponent<Flux>().index = index;
        Vector3 destination = new Vector3(pin.AttachmentPoint.Item1.x, pin.AttachmentPoint.Item1.y, pin.AttachmentPoint.Item1.z - 100f);
        inst.GetComponent<Flux>().destination = destination;
        return inst.GetComponent<Flux>();
    }

    //Starts depletion of the flux on the pin given as input index
    public void StartFluxDepletion(Flux flux)
    {
        flux.startDepletion();
        AudioManager.Instance().StopZap();
        AudioManager.Instance().PlayStartDownload();
        Dude.fluxStartedDepletion = true;
        idleFluxes.Remove(flux);
        depletingFluxes.Add(flux);
    }

    //Pauses depletion of the flux when the cable is detached somewhere
    public void PauseFluxDepletion(Flux flux)
    {
        flux.pauseDepletion();
        AudioManager.Instance().StopStartDownload();
        if (!idleFluxes.Contains(flux))
            idleFluxes.Add(flux);
        if (depletingFluxes.Contains(flux))
            depletingFluxes.Remove(flux);
    }

    //Function used by fluxes to notify the manager that they arrived at the destination
    public void FluxArrivedAtDestination(Flux flux)
    {
        idleFluxes.Add(flux);
        CheckForPossibleDepletionAtArrival(flux);
    }

    //Function used by fluxes to notify the game manager that they are depleted
    public void FluxDepleted(Flux flux)
    {
        depletingFluxes.Remove(flux);
        numberFluxesDepleteded++;
        GameObject.Destroy(flux.gameObject);
        if (!preventFluxSpawning)
        {
            spawnablePins.Add(MapUtility.UpperPins.ElementAt(flux.index));
        }
    }

    //Function to delete a flux (used in tutorials only)
    public void DeleteFlux(Flux flux)
    {
        depletingFluxes.Remove(flux);
        idleFluxes.Remove(flux);
        GameObject.Destroy(flux.gameObject);
    }
    public void CheckForPossibleDepletion(Pin lPin)
    {
        Pin uPin = new Pin();
        foreach (var pin in MapUtility.UpperPins)
        {
            //estraggo il rispettivo upper pin collegato al lower pin
            if (pin.CableConnected != null)
            {
                if (pin.CableConnected.index == lPin.CableConnected.index)
                    uPin = pin;
            }
        }

        //vedo se c'è un flusso in attesa
        var possibleFluxWaiting = idleFluxes.FirstOrDefault(flux => flux.index == uPin.Index);
        if (possibleFluxWaiting != null)
        {
            //vedo se c'è un altro flusso nello stesso pin che nello stesso momento sta venendo esaurito
            var fluxDepletingSameIndex = depletingFluxes.FirstOrDefault(flux => flux.index == possibleFluxWaiting.index);
            //se non lo trovo, e i pin collegati sono dello stesso colore, incomincio ad esaurire il flusso
            if (lPin.Instance.GetComponent<Renderer>().material.color == uPin.Instance.GetComponent<Renderer>().material.color &&
                fluxDepletingSameIndex == null)
                StartFluxDepletion(possibleFluxWaiting);
        }
    }
    public void CheckForPossibleDepletionAtArrival(Flux arrivalFlux)
    {
        //estraggo il rispettivo upper pin collegato al flusso
        var uPin = MapUtility.UpperPins.First(pin => pin.Index == arrivalFlux.index);
        if (uPin.IsConnected)
        {
            //estraggo il rispettivo lower pin collegato al flusso
            var possibleLPin = MapUtility.LowerPins.FirstOrDefault(pin => pin.CableConnected == uPin.CableConnected);
            if (possibleLPin != null && possibleLPin.IsConnected)
            {
                //se non c'è un altro flusso nello stesso pin che nello stesso momento sta venendo esaurito e se i pin collegati sono dello stesso colore
                var fluxDepletingSameIndex = depletingFluxes.FirstOrDefault(flux => flux.index == arrivalFlux.index);
                if (possibleLPin.Instance.GetComponent<Renderer>().material.color == uPin.Instance.GetComponent<Renderer>().material.color &&
                    fluxDepletingSameIndex == null)
                    StartFluxDepletion(arrivalFlux);
            }
        }
    }

    //funzione per stoppare l'esaurimento in corso di un flusso
    public void CheckForPossibleDepletionPauses(Pin lPin)
    {
        var uPin = MapUtility.UpperPins.FirstOrDefault(pin => pin.CableConnected == lPin.CableConnected);
        if (uPin != null)
        {
            var possibleDepletingFlux = depletingFluxes.FirstOrDefault(flux => flux.index == uPin.Index);
            if (possibleDepletingFlux != null)
                PauseFluxDepletion(possibleDepletingFlux);
        }
    }
}
