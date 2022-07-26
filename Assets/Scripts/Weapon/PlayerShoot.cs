using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private const string PLAYER_TAG = "PLAYER";

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();

    }

    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    [Client]
    void Shoot()
    {
        Debug.Log("Shoot");
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
        }

    }

    [Command]
    void CmdPlayerShot(String _playerID, int damage)
    {
        Debug.Log(_playerID + "has been shot.");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(damage);
        //Destroy(GameObject.Find(_ID));
    }
}
