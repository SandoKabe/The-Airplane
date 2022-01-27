using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovePlane : MonoBehaviour
{
    // Used to move and flight the airplane.
    public Transform handle;

    public enum AirState
    {
        Idle,
        TakeOff,
        Flying,
        Landing
    };

    // PUBLIC
    public AirState airState;

    // Vector Translation 
    public float right;
    public float up;
    public float straight;
    // Vector Rotation on X axis
    public float XRotation;
    public float ZRotation;
    // Handle inputs
    public float inputRotationX;
    public float inputRotationZ;


    // PRIVATE
    float minRot = -0.1f;
    float maxRot = 0.1f;
    int counter;
    int countFlying;
    int countToIdle;
    bool hasTooWait;
    bool isFlying;
    Vector3 originPos;
    AudioSource audio;
    
    void Start()
    {
        handle = GetComponentInChildren<AirHandle>().gameObject.transform;
        audio = GetComponent<AudioSource>();
        inputRotationX = 0;
        inputRotationZ = 0;
        airState = AirState.Idle;
        originPos = transform.position;
    }

    private void Update()
    {
        inputRotationX = handle.localRotation.x * 100;
        inputRotationZ = handle.localRotation.z * 100;

        switch (airState)
        {
            case AirState.Idle:
                straight = 0;
                up = 0;
                right = 0;
                Propeller.Instance.StopRotatePorpeller();
                audio.Stop();
                break;
            case AirState.TakeOff:
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
                Propeller.Instance.RotatePorpeller();
                up = 0;
                right = 0;
                straight = 1;
                break;
            case AirState.Flying:
                straight = 1;
                up = inputRotationX == 0 ? 0 : inputRotationX / Mathf.Abs(inputRotationX);
                right = inputRotationZ == 0 ? 0 : inputRotationZ / Mathf.Abs(inputRotationZ);
                right /= 3f;
                up /= 2f;
                XRotation = Mathf.Clamp(inputRotationX, minRot, maxRot);
                ZRotation = Mathf.Clamp(inputRotationZ, minRot, maxRot);
                transform.Rotate(XRotation, 0, ZRotation);
                break;
            case AirState.Landing:
                //straight = 0.5f;
                //up = 0;
                //right = 0;
                straight = 0;
                up = 0;
                right = 0;
                inputRotationX = 0;
                inputRotationZ = 0;
                float dist = Vector3.Distance(originPos, transform.position);
                Debug.Log("dist" + dist);
                transform.LookAt(originPos);
                transform.position = Vector3.Lerp(transform.position, originPos, Time.deltaTime); ;
                if (dist < 10)
                {
                    transform.SetPositionAndRotation(transform.position, new Quaternion(0f, 0f, 0f, 1f));
                    handle.SetPositionAndRotation(handle.position, new Quaternion(0f, 0f, 0f, 1f));
                }
                if (dist < 1)
                {
                    //transform.SetPositionAndRotation(transform.position, new Quaternion(0f, 0f, 0f, 1f));
                    countToIdle++;
                }
                if (countToIdle > 300)
                {
                    countToIdle = 0;
                    audio.Stop();
                    Propeller.Instance.StopRotatePorpeller();
                    //airState = AirState.Idle;
                }
                return;
                break;
            default:
                break;
        }

        transform.Translate(new Vector3(right * 0.1f, up * -0.1f, straight * 0.1f));

        airState = GetState();        
    }

    private AirState GetState()
    {
        bool isOutOfBound = Bound.Instance.OutOfBound(transform.position);
        if (isOutOfBound)
        {
            Debug.Log("Landing");
            return AirState.Landing;
        }
        if (straight > 0 && countFlying < 100)
        {
            isFlying = true;
        }

        hasTooWait = false;

        if (inputRotationX == 0 && airState == AirState.Idle)
        {
            return AirState.Idle;
        }
        if (straight != 0 && counter < 300)
        {
            counter++;
            hasTooWait = true;
            return airState;

        }
        if (!hasTooWait && !isFlying)
        {
            countFlying++;
            return AirState.TakeOff;
        }
        if (Mathf.Abs(inputRotationX) > 20)
        {
            return AirState.Flying;
        }
        if (!hasTooWait && isFlying)
        {
            return AirState.Flying;
        }
        if (straight == 0 && isFlying)
        {
            return AirState.Flying;
        }
        if (transform.position.y < 100)
        {
            return AirState.Landing;
        }

        return AirState.Idle;
    }
}
