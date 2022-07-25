using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get{return _isDead;}
        protected set{ _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

  
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

        SetDefaults();

        Transform respawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        } 

        Collider col = GetComponent<Collider>();
        if(col != null)
            col.enabled = true;
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage)
    {
        if(isDead)
            return;

        currentHealth -= damage;

        Debug.Log(transform.name + "now has " + currentHealth + "HP");

        if(currentHealth <=0)
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

         Collider col = GetComponent<Collider>();
        if(col != null)
            col.enabled = false;
        Debug.Log(transform.name + " is DEAD! ") ;

        StartCoroutine(Respawn());
    }
}   
