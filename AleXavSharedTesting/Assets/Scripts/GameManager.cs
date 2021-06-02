using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    public GameObject fluxPrefab;
    public List<GameObject> spawnPoints;
    private int firstPinIndex = 0;
    private int lastPinIndex = 4;
    private Flux[] idleFluxes;
    static GameManager gm;

    public AudioManager am;
    public SaveManager sm;
    public MenuManager mm;

    void Start()
    {
        Debug.Log("hoga");
        gm = this;

        idleFluxes = new Flux[lastPinIndex + 1];
        StartCoroutine(spawnRandomFluxesForever());
    }
    public static GameManager Instance()
    {
        return gm;
    }

    //logic to decide what to do on flux arrival
    private void onFluxArrival()
    {
        // TODO
    }
    //logic to decide what to do on flux depletion
    private void onFluxDepletion()
    {
        //TODO
    }




    //spawn fluxes on random pins with a fixed delay between them
    private float fluxSpawnDelay = 1;
    IEnumerator spawnRandomFluxesForever()
    {
        while (true)
        {
            int ranInd = Random.Range(0, 5);
            SpawnFluxIndex(ranInd);
            yield return new WaitForSeconds(fluxSpawnDelay);
        }
    }

    //spawn a flux that will arrive on the pin corresponding to the index passed (0-4)
    public void SpawnFluxIndex(int index)
    {
        if (index >= firstPinIndex && index <= lastPinIndex)
        {
            GameObject inst = GameObject.Instantiate(fluxPrefab, spawnPoints[index].transform.position, spawnPoints[index].transform.rotation, this.transform);
            inst.GetComponent<Flux>().index = index;
            inst.GetComponent<Flux>().destination = spawnPoints[index].transform.position + new Vector3(0, 0, 92);
        }
        else
        {
            Debug.LogAssertion("you tried to spawn a flux on a non existing pin: pin n. " + index + " doesn't exist" );
        }
    }
    //Starts depletion of the flux on the pin given as input index
    public void StartFluxDepeletionAtIndex(int index)
    {
        idleFluxes[index].startDepletion();
    }
    //Function used by fluxes to notify the manager that they arrived at the destination
    public void FluxArrivedAtDestination(Flux flux)
    {
        Debug.Log(flux.index);
        Assert.IsNull(idleFluxes[flux.index], "Flux arrived at a pin already occupied");
        idleFluxes[flux.index] = flux;
        AudioManager.Instance().PlayZap();
    }
    //Function used by fluxes to notify the game manager that they are depleted
    public void FluxDepleted(Flux flux)
    {
        GameObject.Destroy(flux.gameObject);
    }

}
