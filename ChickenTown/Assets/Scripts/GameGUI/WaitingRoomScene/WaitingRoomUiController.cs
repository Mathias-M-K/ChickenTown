using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Player = Photon.Realtime.Player;

namespace GameGUI.WaitingRoomScene
{
    public class WaitingRoomUiController : MonoBehaviour
    {
        public WaitingRoomController roomController;
        public GameObject playerList;
        public GameObject playerNameElement;
        private readonly Dictionary<string, GameObject> _activePlayers = new Dictionary<string, GameObject>();
        
        public void AddPlayerToList(Player player)
        {
            GameObject go = Instantiate(playerNameElement, playerList.transform, false);
            go.GetComponent<TextMeshProUGUI>().text = player.NickName;
            
            _activePlayers.Add(player.NickName,go);
        }

        public void RemovePlayerFromList(Player player)
        {
            GameObject go = _activePlayers[player.NickName];
            _activePlayers.Remove(player.NickName);
            DestroyImmediate(go,true);
            Destroy(go);
        }
    }
}