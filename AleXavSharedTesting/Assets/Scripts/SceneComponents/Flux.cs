using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flux : MonoBehaviour
{
    public Vector3 destination;
    public float speed;
    public float scalingSpeed;
    public Color color;
    public GameObject trailScaler;
    public GameObject trail;
    public GameObject tip;
    public float arrivalAcceptanceMargin;
    public float scaleAcceptanceLevel;
    public int index;
    public float requestTimer;

    public bool hasArrived { get; set; }
    private FluxLifeState state = FluxLifeState.Moving;
    void Start()
    {
        this.transform.LookAt(destination);
        requestTimer = 0;
        hasArrived = false;
    }
    // Based on which state the flux is either does nothing or moves towards the destination or depletes the trail
    void Update()
    {
        if (!MapUtility.GamePaused)
        {
            switch (state)
            {
                case FluxLifeState.Idle:
                    break;
                case FluxLifeState.Moving:
                    //se il flusso si sta muovendo, si sposta verso la destinazione alla velocita' inserita
                    if ((destination - trailScaler.transform.position).magnitude > arrivalAcceptanceMargin)
                        this.transform.position += (destination - this.transform.position).normalized * speed * Time.deltaTime;
                    else
                    {
                        tip.SetActive(false);
                        state = FluxLifeState.Idle;
                        GameManager.Instance().FluxArrivedAtDestination(this);
                        hasArrived = true;
                    }
                    break;
                case FluxLifeState.Depleting:
                    //se il flusso si sta consumando, si accorcia (scala negativamente) alla velocita' inserita
                    if (trailScaler.transform.localScale.z > scaleAcceptanceLevel)
                        trailScaler.transform.localScale = trailScaler.transform.localScale - new Vector3(0, 0, scalingSpeed) * Time.deltaTime;
                    else
                    {
                        state = FluxLifeState.Depleted;
                        GameManager.Instance().FluxDepleted(this);
                    }
                    break;
                case FluxLifeState.Depleted:
                    break;
            }
        }
    }
    public void startDepletion()
    {
        state = FluxLifeState.Depleting;
        //Debug.Log("Start depletion");
    }

    public void pauseDepletion()
    {
        state = FluxLifeState.Idle;
        //Debug.Log("Stop depletion");
    }
}
