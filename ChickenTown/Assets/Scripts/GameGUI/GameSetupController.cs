using System.IO;
using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public class GameSetupController : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            if (!PhotonNetwork.IsConnected) return;
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            Debug.Log("Creating Player");
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), new Vector3(0,0,0), Quaternion.identity);
        }
    }
}