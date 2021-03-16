using System;

using ExitGames.Client.Photon;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using System.Linq;

namespace Com.CharismaZero.MathyMaze
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        public GameObject refCamera;
        public static GameObject localCamera;
        public static int seed;

        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion


        #region Public Methods

        public void Start()
        {
            localCamera = refCamera;
            /*
            seed = UnityEngine.Random.Range(-30000,30001);

            int i = 0;
            Hashtable hashtable = new Hashtable();
            hashtable.Add("Seed", seed);
            while (i < PhotonNetwork.PlayerList.Length)
            {
                PhotonNetwork.PlayerList[i].SetCustomProperties(hashtable);
            }

            */
            if (ControlScript.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

                if (playerPrefab == null)
                {
                    Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
                }
                else
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer");
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    GameObject temp = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                   
                    /*
                    temp.transform.position = new Vector3(-6, 0, GameController.startZ);
                    temp.transform.Rotate(Vector3.up, 90);
                    */
                    
                }
                
            }
            else
            {
                //localCamera.transform.parent = ControlScript.LocalPlayerInstance.transform;
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("SampleScene");
        }


        #endregion
        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        #endregion
    }
}