using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componetsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    Camera sceneCamera;

     void Start()
     {
        if(!isLocalPlayer)
        {
            DisableComponets();
            AssignRemoteLayer();
        }
         else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
        
        RegisterPlayer();
     }

     void RegisterPlayer()
     {
        string ID = "Player" + GetComponent<NetworkIdentity>().netId;
        transform.name = ID;
     }

     void DisableComponets()
     {
        for(int i = 0; i < componetsToDisable.Length;i++)
        {
            componetsToDisable[i].enabled = false;
        }
     }

     void AssignRemoteLayer()
     {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
     }

     void OnDisable()
     {
        if(sceneCamera != null)
        {
                sceneCamera.gameObject.SetActive(true);
        }
     }
}
