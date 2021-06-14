using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private List<Flux> idleFluxes;
    private List<Flux> depletioningFluxes;

    public GameObject fluxPrefab;
    //spawnPoints presi dagli attachment dello scenario, andr� tolto
    public List<GameObject> spawnPoints;
    public Sprite[] livesSprites = new Sprite[3];
    public SpriteRenderer livesIndicator;
    public bool preventLoosingLife;
    public bool preventFluxSpawning;

    private int maxLives = 3;
    private int lives;
    private int numberFluxesDepleteded;


    public static GameManager gm;

    void Start()
    {
        gm = this;
        lives = maxLives;
        numberFluxesDepleteded = 0;

        MapUtility.LowerPins = new List<Pin>();
        MapUtility.UpperPins = new List<Pin>();

        var lowPins = MapUtility.GetAllObjectsOnlyInScene().Where(x => x.name.Contains("LPin")).OrderBy(pin => pin.name).ToList();
        var upperPins = MapUtility.GetAllObjectsOnlyInScene().Where(x => x.name.Contains("UPin")).OrderBy(pin => pin.name).ToList();

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

            MapUtility.LowerPins.Add(pinInstance);
            pinIndex++;
        }

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

            MapUtility.UpperPins.Add(pinInstance);
            pinIndex++;
        }
        idleFluxes = new List<Flux>();
        depletioningFluxes = new List<Flux>();

        MapUtility.collisionMapBaseSetup();
        MapUtility.setCollisionMap(200, -500, CollisionEntity.getFullCollisionEntity());
        MapUtility.setCollisionMap(0, -500, CollisionEntity.getFullCollisionEntity());
        MapUtility.setCollisionMap(-200, -500, CollisionEntity.getFullCollisionEntity());
        MapUtility.setCollisionMap(200, 500, CollisionEntity.getFullCollisionEntity());
        MapUtility.setCollisionMap(0, 500, CollisionEntity.getFullCollisionEntity());
        MapUtility.setCollisionMap(-200, 500, CollisionEntity.getFullCollisionEntity());
        MapUtility.setCollisionMap(200, 200, new Bridge());

        if(!preventFluxSpawning)
            StartCoroutine(spawnRandomFluxesForever());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            MenuManager.Instance().LoadPauseMenu();

        foreach (var lPin in MapUtility.LowerPins.Where(pin => pin.IsConnected))
            CheckForPossibleDepletion(lPin);

        foreach (var flux in idleFluxes)
        {
            AudioManager.Instance().PlayZap();

            flux.requestTimer += Time.deltaTime;

            if (flux.requestTimer >= 5)
            {
                if(!preventLoosingLife)
                    LoseLives(1);
                flux.requestTimer = 0;
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
    public void LoseLives(int amount)
    {
        //Debug.Log("loseLife: " + lives);
        lives -= amount;
        if (lives > 0)
        {
            livesIndicator.sprite = livesSprites[lives - 1];
            AudioManager.Instance().PlayLoseLife();
        }
        else
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        Debug.Log("GameOver");
    }
    //spawn fluxes on random pins with a fixed delay between them
    private float fluxSpawnDelay = 20;
    IEnumerator spawnRandomFluxesForever()
    {
        while (true)
        {
            int ranInd = UnityEngine.Random.Range(0, MapUtility.UpperPins.Count);
            SpawnFluxIndex(ranInd);
            Debug.Log("Spawned at index: " + ranInd);
            yield return new WaitForSecondsRealtime(fluxSpawnDelay);
        }
    }

    //spawn a flux that will arrive on the pin corresponding to the index passed
    public void SpawnFluxIndex(int index)
    {
        var pin = MapUtility.UpperPins.FirstOrDefault(pinA => pinA.Index == index);

        GameObject inst = GameObject.Instantiate(fluxPrefab, pin.FluxSpawnPoint.Item1, pin.FluxSpawnPoint.Item2, this.transform);
        inst.GetComponent<Flux>().index = index;
        inst.GetComponent<Flux>().destination = pin.AttachmentPoint.Item1;
    }

    //Starts depletion of the flux on the pin given as input index
    public void StartFluxDepletion(Flux flux)
    {
        flux.startDepletion();
        AudioManager.Instance().StopZap();
        AudioManager.Instance().PlayStartDownload();
        idleFluxes.Remove(flux);
        depletioningFluxes.Add(flux);
        //onFluxDepletion();
    }

    //Pauses depletion of the flux when the cable is detached somewhere
    public void PauseFluxDepletion(Flux flux)
    {
        flux.pauseDepletion();
        AudioManager.Instance().StopStartDownload();
        if (!idleFluxes.Contains(flux))
            idleFluxes.Add(flux);
        if(depletioningFluxes.Contains(flux))
            depletioningFluxes.Remove(flux);
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
        depletioningFluxes.Remove(flux);
        numberFluxesDepleteded++;
        GameObject.Destroy(flux.gameObject);
    }

    public void CheckForPossibleDepletion(Pin lPin)
    {
        var uPin = MapUtility.UpperPins.First(pin => pin.CableConnected == lPin.CableConnected);

        var possibleFluxWaiting = idleFluxes.FirstOrDefault(flux => flux.index == uPin.Index);

        if(possibleFluxWaiting != null)
        {
            var fluxDepletingSameIndex = depletioningFluxes.FirstOrDefault(flux => flux.index == possibleFluxWaiting.index);

            if (lPin.Instance.GetComponent<Renderer>().material.color == uPin.Instance.GetComponent<Renderer>().material.color &&
                fluxDepletingSameIndex == null)
                StartFluxDepletion(possibleFluxWaiting);
        }
    }
    public void CheckForPossibleDepletionAtArrival(Flux arrivalFlux)
    {
        var uPin = MapUtility.UpperPins.First(pin => pin.Index == arrivalFlux.index);

        if (uPin.IsConnected)
        {
            var possibleLPin = MapUtility.LowerPins.FirstOrDefault(pin => pin.CableConnected == uPin.CableConnected);


            if(possibleLPin != null && possibleLPin.IsConnected)
            {
                var fluxDepletingSameIndex = depletioningFluxes.FirstOrDefault(flux => flux.index == arrivalFlux.index);

                if (possibleLPin.Instance.GetComponent<Renderer>().material.color == uPin.Instance.GetComponent<Renderer>().material.color &&
                    fluxDepletingSameIndex == null)
                    StartFluxDepletion(arrivalFlux);
            }
        }
    }

    public void CheckForPossibleDepletionPauses(Pin lPin)
    {
        var uPin = MapUtility.UpperPins.FirstOrDefault(pin => pin.CableConnected == lPin.CableConnected);
        if(uPin != null)
        {
            var possibleDepletingFlux = depletioningFluxes.FirstOrDefault(flux => flux.index == uPin.Index);

            if (possibleDepletingFlux != null)
                PauseFluxDepletion(possibleDepletingFlux);
        }
    }
}
