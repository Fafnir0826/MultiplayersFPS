using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentcameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float cameraRotationLimit = 85f;
    private Rigidbody rb;
    

    void Awake()
    {
       rb = GetComponent<Rigidbody>();

    }
      void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

   

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
     public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
     public void RotateCamera(float _cameraRotation)
    {
        cameraRotationX = _cameraRotation;
    }
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }
 
    void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if(thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime,ForceMode.Acceleration);
        }
    }
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if(cam != null)
        {
           currentcameraRotationX -= cameraRotationX;
           currentcameraRotationX = Mathf.Clamp(currentcameraRotationX, -cameraRotationLimit,cameraRotationLimit);

           cam.transform.localEulerAngles = new Vector3(currentcameraRotationX, 0f, 0f);
        }
    }


}
