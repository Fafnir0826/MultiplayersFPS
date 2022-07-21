using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    public PlayerWeapon weapon;
    private const string PLAYER_TAG = "PLAYER";
    void Start()
    {
        if(cam == null)
        {
                Debug.LogError("PlayerShoot: No camera referenced!");
                this.enabled = false;
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            if(hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name);
            }
        }

    }

    [Command]
    void CmdPlayerShot(String _ID)
    {
        Debug.Log(_ID + "has been shot.");

        //Destroy(GameObject.Find(_ID));
    }
}
