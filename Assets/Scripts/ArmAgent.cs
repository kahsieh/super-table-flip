using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ArmAgent : Agent
{
    public GameObject leftArm;
    public GameObject rightArm;
    public Collider[] leftColliders;
    public Collider[] rightColliders;

    public GameObject table;
    public GameObject[] otherObjects;

    public Dictionary<int, Vector3> idToPosition;
    public Dictionary<int, Quaternion> idToRotation;

    public float speed = 100;
    public float timer = 0;
    public bool startTimer = false;
    // Start is called before the first frame update

    void Start()
    {
        idToPosition = new Dictionary<int, Vector3>();
        idToRotation = new Dictionary<int, Quaternion>();
        foreach (GameObject obj in otherObjects)
        {
            idToPosition.Add(obj.GetInstanceID(), obj.transform.localPosition);
            idToRotation.Add(obj.GetInstanceID(), obj.transform.rotation);
        }
    }

    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
        }
    }

    public override void OnEpisodeBegin()
    {
        //reset position
        Debug.Log("start");
        foreach (GameObject obj in otherObjects)
        {
            obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.transform.localPosition = idToPosition[obj.GetInstanceID()];
            obj.transform.rotation = idToRotation[obj.GetInstanceID()];
        }

        //reset table boolean
        table.GetComponent<table>().Reset();


        startTimer = true;
        timer = 0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (GameObject obj in otherObjects)
        {
            sensor.AddObservation(obj.transform.position);
            sensor.AddObservation(obj.GetComponent<Rigidbody>().velocity);
            sensor.AddObservation(obj.transform.rotation);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
      
        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];
        controlSignal.z = vectorAction[2];
        Debug.Log(controlSignal);
        Vector3 controlSignal2 = Vector3.zero;
        controlSignal2.x = vectorAction[3];
        controlSignal2.y = vectorAction[4];
        controlSignal2.z = vectorAction[5];
        leftArm.GetComponent<Rigidbody>().AddForce(controlSignal * speed);
        rightArm.GetComponent<Rigidbody>().AddForce(controlSignal2 * speed);


        if (timer > 10f)
        {
            // ends if timeout
            startTimer = false;
            Debug.Log("end");
            EndEpisode();
        }

        // ends if the arm moves too far away
        //TODO  should have a contraint between the two arms 
        if (leftArm.transform.localPosition.x > 3 || leftArm.transform.localPosition.x < -3 || leftArm.transform.localPosition.z > 3 || leftArm.transform.localPosition.z < -3 || leftArm.transform.localPosition.y > 3 || leftArm.transform.localPosition.y < -3)
        {
            EndEpisode();
        }
        if (rightArm.transform.localPosition.x > 3 || rightArm.transform.localPosition.x < -3 || rightArm.transform.localPosition.z > 3 || rightArm.transform.localPosition.z < -3 || rightArm.transform.localPosition.y > 3 || rightArm.transform.localPosition.y < -3)
        {
            EndEpisode();
        }

        // giving a negative reward every timeframe - probably unnecessary
        SetReward(-0.1f);
    }

    // for external objects use
    public void externalSetReward(float reward)
    {
        SetReward(reward);
    }

    public override void Heuristic(float[] actionsOut)
    {
       
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        actionsOut[2] = Input.GetAxis("Horizontal");
        actionsOut[3] = Input.GetAxis("Vertical");
        actionsOut[4] = Input.GetAxis("Horizontal");
        actionsOut[5] = Input.GetAxis("Vertical");
    }
}
