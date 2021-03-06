﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ParachuteLogic : MonoBehaviour
{
    public GameObject payload;
    public Vector3 windForce;
    public GameObject rigidParachute;
    public float maximumParachuteUnitsOfLift = 1.0f;
    public float slowestFallingSpeed = 5.0f;
    public Material cableMaterial;
    private Transform[] parachuteCableHookPoints;
    private Transform[] payloadCableHookPoints;
    public float[] prevCableLengthDistance;
    private GameObject parachuteCablesContainer;
    private ParachutePayloadLogic payloadComp;
    private LineRenderer[] cables;

    public void Start()
    {
        payloadComp = payload.AddComponent<ParachutePayloadLogic>();
        payloadComp.coreParachuteLogic = this;

        rigidParachute.GetComponent<ConfigurableJoint>().connectedBody = payload.GetComponent<Rigidbody>();

        CreateCables();
    }

    private void CreateCables()
    {
        Transform parachuteCableHookPointsParent = rigidParachute.transform.Find("CableHookPoints");
        Transform payloadCableHookPointsParent = payload.transform.Find("CableHookPoints");
        LineRenderer cable;

        parachuteCablesContainer = new GameObject();
        parachuteCablesContainer.name = "Cables";
        parachuteCablesContainer.transform.parent = transform;

        parachuteCableHookPoints = new Transform[parachuteCableHookPointsParent.GetChildCount()];
        payloadCableHookPoints = new Transform[payloadCableHookPointsParent.GetChildCount()];

        for (int t = 0; t < parachuteCableHookPointsParent.GetChildCount(); t++)
        {
            parachuteCableHookPoints[t] = parachuteCableHookPointsParent.GetChild(t);
        }

        for (int t = 0; t < payloadCableHookPointsParent.GetChildCount(); t++)
        {
            payloadCableHookPoints[t] = payloadCableHookPointsParent.GetChild(t);
        }

        if (parachuteCableHookPoints.Length != payloadCableHookPoints.Length)
        {
            Debug.Log("Cable Hook Points on the Parachute need to match the payload cable hook points");
        }

        cables = new LineRenderer[parachuteCableHookPoints.Length];
        prevCableLengthDistance = new float[parachuteCableHookPoints.Length];

        for (int t = 0; t < parachuteCableHookPoints.Length; t++)
        {
            cable = new GameObject().AddComponent<LineRenderer>();
            cable.material = cableMaterial;
            cable.transform.parent = parachuteCablesContainer.transform;
            cable.transform.name = parachuteCableHookPoints[t].name + "Cable";
            
            cable.transform.position = parachuteCableHookPoints[t].position;
            prevCableLengthDistance[t] = 0;
            cable.SetWidth(.04f, .065f);
            cables[t] = cable;
        }
    }

    public void Update()
    {
    	UpdateCables();
    }

    private void UpdateCables()
    {
        LineRenderer cable;

        for (int t = 0; t < cables.Length; t++)
        {
            cable = cables[t];
            cable.SetPosition(0, parachuteCableHookPoints[t].position);
            cable.SetPosition(1, payloadCableHookPoints[t].position);
        }
    }


    public void payloadCollisionEnter(Collision collision)
    {
        rigidParachute.SetActiveRecursively(false);
    }

    public void FixedUpdate()
    {
        float angleDiffFromVertical = Vector3.Angle(Vector3.up, rigidParachute.transform.up);
        float parchuteSurfaceArea;

        if (angleDiffFromVertical <= 90) //bottom down
        {
            parchuteSurfaceArea = Mathf.Cos(angleDiffFromVertical) * 1; //doesn't matter show wide the parachute is
        }
        else
        {
            parchuteSurfaceArea = Mathf.Cos(180-angleDiffFromVertical) * 1;
            parchuteSurfaceArea /= 5; //way less lift if upside down
        }

        float additionalUpwardForce = -Physics.gravity.y - slowestFallingSpeed;
        additionalUpwardForce = additionalUpwardForce * parchuteSurfaceArea;
        Vector3 appliedForceAngle = Vector3.Slerp(rigidParachute.transform.up, Vector3.up, 0.33f);


        rigidParachute.GetComponent<Rigidbody>().AddForce(appliedForceAngle * (-rigidParachute.GetComponent<Rigidbody>().velocity.y + additionalUpwardForce));
    }


}
