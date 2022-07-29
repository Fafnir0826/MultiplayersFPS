using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;



public class JoinGame : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;
    [SerializeField]
    private GameObject roomListItmePrefab;
    [SerializeField]
    private Transform roomListParent;
    private NetworkManager networkManager;
    
    void Start()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        networkManager.matchMaker.ListMatches(0,20,"",true,0,0, OnMatchList);
        status.text ="Loading...";
    }

    public void OnMatchList (bool success, string extendedInfo,List<MatchInfoSnapshot> matchList)
    {
        status.text = "";
        if(matchList == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }

        ClearRoomList();

        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItmePrefab);
            _roomListItemGO.transform.SetParent(roomListParent);
            
            roomList.Add(_roomListItemGO);
        }
        
        if(roomList.Count <= 0)
        {
            status.text = "No rooms at the moment";
        }
    }

    void ClearRoomList()
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }
}
