using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    Collider collider;
    private Rigidbody rBody;
    bool gameEnds = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rBody = GetComponent<Rigidbody>();
    }

    // public override void OnEpisodeBegin()
    // {
    //     /*
    //     if () // Some condition when the game ends
    //     {
    //         gameEnds = false;
    //         // If the Agent fell, zero its momentum
    //         this.rBody.angularVelocity = Vector3.zero;
    //         this.rBody.velocity = Vector3.zero;
    //         this.transform.localPosition = new Vector3( 0, 0.5f, 0);
    //     }
    //     */
    // }

    // public override void CollectObservations(VectorSensor sensor)
    // {
    //     //Todo:
    //     // I think of the getting observations of: 
    //         // 1. velocity, angular velocity, position of both hands
    //         // 2. velocity, angular velocity, position of the table
    //         // 3. All positions of objects can be collide in the scene


    //     // Target and Agent positions
    //     // sensor.AddObservation(Target.localPosition);
    //     // sensor.AddObservation(this.transform.localPosition);

    //     // // Agent velocity
    //     // sensor.AddObservation(rBody.velocity.x);
    //     // sensor.AddObservation(rBody.velocity.z);
    // }

    public float speed = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 3
        // Todo: Actions for both hand, now it only controls one hand signal
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];
        controlSignal.z = vectorAction[2];
        rBody.AddForce(controlSignal * speed);

        // Rewards
        int reward = 0;
        // Todo: if(table is not near the player, and its speed is close to 0): {
            // reward = calculate all the colision scores it made
            // gameEnds = true;
        // }
        // float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (gameEnds)
        {
            SetReward(reward);
            EndEpisode();
        }

        // Push the agent to flip the table as soon as possible
        SetReward(-0.01f);
    }

    // Manual Control
    // public override void Heuristic(float[] actionsOut)
    // {
    //     // actionsOut[0] = Input.GetAxis("Horizontal");
    //     // actionsOut[1] = Input.GetAxis("Vertical");
    // }
}
