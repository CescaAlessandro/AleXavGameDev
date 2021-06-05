using System.Collections;
using System.Collections.Generic;
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
    private FluxLifeState state = FluxLifeState.Moving;
    void Start()
    {
        this.transform.LookAt(destination);
        requestTimer = 0;
    }
    // Based on which state the flux is either does nothing or moves towards the destination or depletes the trail
    void Update()
    {
        switch (state)
        {
            case FluxLifeState.Idle:
                break;
            case FluxLifeState.Moving:
                if ((destination - trailScaler.transform.position).magnitude > arrivalAcceptanceMargin)
                    this.transform.position += (destination - this.transform.position).normalized * speed * Time.deltaTime;
                else
                {
                    tip.SetActive(false);
                    state = FluxLifeState.Idle;
                    GameManager.Instance().FluxArrivedAtDestination(this);
                }
                break;
            case FluxLifeState.Depleting: 
                if (trailScaler.transform.localScale.z > scaleAcceptanceLevel)
                    trailScaler.transform.localScale = trailScaler.transform.localScale - new Vector3(0, 0, scalingSpeed) * Time.deltaTime;
                else
                {
                    state = FluxLifeState.Idle;
                    GameManager.Instance().FluxDepleted(this);
                }
                break;
            case FluxLifeState.Depleted:
                break;
        }
    }

    public void startDepletion()
    {
        state = FluxLifeState.Depleting;
    }
}
