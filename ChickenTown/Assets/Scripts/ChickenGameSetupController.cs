using System.IO;
using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public class ChickenGameSetupController : MonoBehaviourPunCallbacks
    {
        private PhotonView myPhotonView;
        
        private void Start()
        {
            //if (GameHandler.gameHandler.gameType == GameType.Local) return;
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            Debug.Log("Creating Player");

            int xSpawnPos = Random.Range(0, 14);
            int zSpawnPos = Random.Range(-13, 5);
            PhotonNetwork.Instantiate(Path.Combine("ChickenPrefabs", "ChickenPlayer"), new Vector3(xSpawnPos,2,zSpawnPos), Quaternion.identity);
        }
    }
}