using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;
    private bool[] wasEnabled;


    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private GameObject spawnEffect;



    /*void Update()
    {
        if(!isLocalPlayer)
            return;
        if(Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(1000);
        }
    }*/

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        Transform respawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        SetDefaults();
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

         GameObject _spawnIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_spawnIns, 3f);
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log(transform.name + "now has " + currentHealth + "HP");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }


        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }
        StartCoroutine(Respawn());
    }
}
