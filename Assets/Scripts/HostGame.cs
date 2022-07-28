using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 6;

    private string roomName;

    private NetworkManager networkManager;

   
    void Start()
    {
        networkManager =  NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }
    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoom()
    {
        if(roomName !="" && roomName !=null)
        {
            Debug.Log("Create Room" + roomName + "with room for" + roomSize + "players");
            //Create Room
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "",0,0, networkManager.OnMatchCreate);
        }
    }

}
