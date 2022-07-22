using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
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

        GetComponent<Player>().Setup();
     }
     public override void OnStartClient()
     {
         base.OnStartClient();

         string _netID = GetComponent<NetworkIdentity>().netId.ToString();
         Player _player = GetComponent<Player>();
         GameManager.RegisterPlayer(_netID, _player);
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

        GameManager.UnRegisterPlayer(transform.name);
     }
}
