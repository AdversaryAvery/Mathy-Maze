using Com.CharismaZero.MathyMaze;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ControlScript : MonoBehaviourPun
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    public WallStopper frontStop;
    public WallStopper backStop;

    public List<Material> materialList;
    public static int playerNum = 0;
    public GameObject playerToken;

    public GameObject localCamera;

    public Text listText;
    public bool hasWon = false;
    // Start is called before the first frame update
    void Start()
    {
        hasWon = false;
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            localCamera.SetActive(false);
        }
        else { 
            GameManager.localCamera.transform.parent = transform;
        }
        transform.position = new Vector3(-6, 0, GameController.startZ);
        transform.Rotate(Vector3.up, 90);

        playerToken.GetComponent<Renderer>().material = materialList[playerNum];
        playerNum = (playerNum + 1) % materialList.Count;
    }


    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Forward();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Backward();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LeftTurn();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightTurn();
            }
        }

        if (photonView.IsMine)
        {
            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            int playerNum;
            if (hashtable.ContainsKey("playerNum"))
            {
                playerNum = (int)hashtable["playerNum"];
            }
            else
            {
                playerNum = 1;
            }
            listText.text = "Player Name\tRank\n";
            
            int index = 1;
            while (index < playerNum)
            {
                if (hashtable.ContainsKey(hashtable["playerName" + index.ToString()]))
                {
                    listText.text += hashtable["playerName" + index.ToString()] + "\t\t" + hashtable[hashtable["playerName" + index.ToString()]].ToString() + "\n";
                }
                else { listText.text += hashtable["playerName" + index.ToString()] + "\n"; }

                index++;

            }

        }
    }

    public void Forward() {
        if (!frontStop.GetBlocked())
        {
            transform.position += transform.forward * 2;
        }
    }

    public void Backward() {
        if (!backStop.GetBlocked())
        {
            transform.position -= transform.forward * 2;
        }
    }

    public void LeftTurn() { 
        transform.Rotate(0f, -90f, 0f); 
    }

    public void RightTurn()
    {
        transform.Rotate(0f, 90f, 0f);
    }
    public void Awake()
    {
        
        if (photonView.IsMine)
        {
            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            int playerNum;
            if (hashtable.ContainsKey("playerNum"))
            {
                playerNum = (int)hashtable["playerNum"];
            }
            else
            {
                playerNum = 1;
            }

            hashtable.Add("playerName" + playerNum, PlayerPrefs.GetString("PlayerName"));
            playerNum++;
            hashtable["playerNum"] = playerNum;
                LocalPlayerInstance = this.gameObject;
            //GameManager.localCamera.transform.parent = transform;
            //transform.position = new Vector3(-6, 0, GameController.startZ);
            //transform.Rotate(Vector3.up, 90);
            listText.text = "Player Name\tRank\n";
            
            int index = 1;
            while (index < playerNum)
            {
                if (hashtable.ContainsKey(hashtable["playerName" + index.ToString()]))
                {
                    listText.text += hashtable["playerName" + index.ToString()] + "\t\t" + hashtable[hashtable["playerName" + index.ToString()]].ToString() + "\n";
                }
                else { listText.text += hashtable["playerName" + index.ToString()] + "\n"; }
                
                index++;
                
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
        
        
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Win") && !hasWon && photonView.IsMine)
        {
            Win();
        }
    }

    public void Win()
    {
        if (photonView.IsMine)
        {
            hasWon = true;
            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            int rank;
            if (hashtable.ContainsKey("Rank"))
            {
                rank = (int)hashtable["Rank"];
            }
            else
            {
                rank = 1;
                hashtable.Add("Rank", rank);
            }

            hashtable.Add(PlayerPrefs.GetString("PlayerName"), rank);
            rank++;
            hashtable["Rank"] = rank;

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }
}
