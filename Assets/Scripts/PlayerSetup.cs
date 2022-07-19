using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componetsToDisable;
    
    Camera sceneCamera;

     void Start()
     {
         if(!isLocalPlayer)
         {
            for(int i = 0; i < componetsToDisable.Length;i++)
            {
                componetsToDisable[i].enabled = false;
            }
         }
         else
         {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
         }
     }
     

     void OnDisable()
     {
        if(sceneCamera != null)
        {
                sceneCamera.gameObject.SetActive(true);
        }
     }
}
