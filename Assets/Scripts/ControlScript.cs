using Com.CharismaZero.MathyMaze;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

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

    public static int pos1;
    public static int pos2;

    public bool facingUp = false;
    public bool facingDown = false;
    public bool facingRight = false;
    public bool facingLeft = false;

    public TextMeshProUGUI roomName;
    public TMP_InputField roomNameCopyPaste;

    public GameObject readyButton;
    public Text readyText;

    public static bool isReady;

    public int selfNumber;
    private int rank = -1;

    // Start is called before the first frame update
    void Start()
    {
        isReady = false;
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

        pos1 = -3;
        pos2 = GameController.startZ / 2;

        facingRight = true;

        roomName.text = PhotonNetwork.CurrentRoom.Name;
        roomNameCopyPaste.text = PhotonNetwork.CurrentRoom.Name;
        roomNameCopyPaste.SetTextWithoutNotify(PhotonNetwork.CurrentRoom.Name);
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
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Forward();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Backward();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                LeftTurn();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                RightTurn();
            }
        }

        if (photonView.IsMine)
        {
            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            int playerNum;

            hashtable[PhotonNetwork.NickName] = rank;
            if (hashtable.ContainsKey("playerNum"))
            {
                playerNum = (int)hashtable["playerNum"];
            }
            else
            {
                playerNum = 1;
            }
            listText.text = "Player \t\tRank\n";
            
            int index = 1;
            while (index < PhotonNetwork.CurrentRoom.PlayerCount + 1)
            {
                if (hashtable.ContainsKey("playerName" + index.ToString()) && (int)hashtable[hashtable["playerName" + index.ToString()]] != -1) {

                        listText.text += hashtable["playerName" + index.ToString()] + "\t\t" + hashtable[hashtable["playerName" + index.ToString()]].ToString() + "\n";
                    }
                else { listText.text += hashtable["playerName" + index.ToString()] + "\t\t\n"; } 
                

                index++;

            }

            if (!GameController.isStarted)
            {
                readyText.text = "Players Ready: " + ((int)hashtable["Ready Players"]).ToString() + "/" + PhotonNetwork.CurrentRoom.PlayerCount + "\n";

                if (GameController.readyPlayers > 0)
                {
                    readyText.text += "Time until Start: " + ((int)GameController.timer).ToString() + "s";
                }
            }
            else
            {
                readyText.text = "Time: " + ((int)GameController.timer).ToString() + "s";
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }

    }

    public void Forward() {
        if (!frontStop.GetBlocked())
        {
            transform.position += transform.forward * 2;

            if (facingRight)
            {
                pos1++;
            }
            else if (facingLeft)
            {
                pos1--;
            }
            else if (facingUp)
            {
                pos2++;
            }
            else if (facingDown)
            {
                pos2--;
            }
        }
    }

    public void Backward() {
        if (!backStop.GetBlocked())
        {
            transform.position -= transform.forward * 2;

            if (facingRight)
            {
                pos1--;
            }
            else if (facingLeft)
            {
                pos1++;
            }
            else if (facingUp)
            {
                pos2--;
            }
            else if (facingDown)
            {
                pos2++;
            }
        }
    }

    public void LeftTurn() { 
        transform.Rotate(0f, -90f, 0f);

        if (facingRight)
        {
            facingRight = false;
            facingUp = true;
        }
        else if (facingLeft)
        {
            facingLeft = false;
            facingDown = true;
        }
        else if (facingUp)
        {
            facingUp = false;
            facingLeft = true;
        }
        else if (facingDown)
        {
            facingDown = false;
            facingRight= true;
        }
    }

    public void RightTurn()
    {
        transform.Rotate(0f, 90f, 0f);

        if (facingRight)
        {
            facingRight = false;
            facingDown = true;
        }
        else if (facingLeft)
        {
            facingLeft = false;
            facingUp = true;
        }
        else if (facingUp)
        {
            facingUp = false;
            facingRight = true;
        }
        else if (facingDown)
        {
            facingDown = false;
            facingLeft = true;
        }
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

            if (PlayerPrefs.GetString("PlayerName") == "" || PlayerPrefs.GetString("PlayerName") == null) {
                PlayerPrefs.SetString("PlayerName", "Unnamed");
            }

            if (hashtable.ContainsKey(PlayerPrefs.GetString("PlayerName"))) {
                int i = 2;
                while(hashtable.ContainsKey(PlayerPrefs.GetString("PlayerName") + i.ToString()))
                {
                    i++;
                }
                PlayerPrefs.SetString("PlayerName", PlayerPrefs.GetString("PlayerName") + i.ToString());

            }
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
            hashtable.Add("playerName" + playerNum.ToString(), PhotonNetwork.NickName);
            hashtable.Add(PhotonNetwork.NickName, 0);

            selfNumber = playerNum;
            playerNum++;
            hashtable["playerNum"] = playerNum;
            LocalPlayerInstance = this.gameObject;

            listText.text = "Player \t\tRank\n";
            
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

            hashtable[PhotonNetwork.NickName] = rank;
            this.rank = rank;
            rank++;
            hashtable["Rank"] = rank;

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }

    public void Ready()
    {
        
            isReady = true;

            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;

            readyButton.SetActive(false);
            hashtable.Add("Ready" + selfNumber.ToString(), 1);

            

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        
    }
}
