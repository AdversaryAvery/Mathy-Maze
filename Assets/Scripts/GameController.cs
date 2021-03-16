using Com.CharismaZero.MathyMaze;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviourPun
{

    public int mazeWidth = 15;
    public int mazeHeight = 15;

    private int[,] maze;

    public GameObject wall;
    public GameObject space;
    public GameObject altSpace;
    public GameObject questionLock;
    public GameObject startZone;
    public GameObject endZone;

    public static int startX = 0;
    public static int startZ = 0;

    private int questionCount = 1;

    public static GameObject exists;
    // Start is called before the first frame update
    void Start()
    {
        if (exists) { Destroy(this); }
        else
        {

            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;

            //hashtable.Add("Seed", seed);

            exists = gameObject;
            /*
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }*/
            maze = new int[mazeHeight, mazeHeight];
            //Random.InitState(GameManager.seed);

            int seed;

            if (!hashtable.ContainsKey("Seed"))
            {
                seed = UnityEngine.Random.Range(-30000, 30001);

                //Hashtable hashtable = new Hashtable();
                hashtable.Add("Seed", seed);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

            }
            else {
                seed = (int) hashtable["Seed"];
            }
            Random.InitState(seed);
            int i, j;

            for (i = 0; i < mazeHeight; i++)
            {
                for (j = 0; j < mazeWidth; j++)
                {
                    if (j == 0 || j == mazeWidth - 1 || i == 0 || i == mazeHeight - 1)
                    {
                        maze[i, j] = 2;
                    }
                    else { maze[i, j] = 0; }


                }
            }

            List<int[]> edgeList = new List<int[]>();

            int x, y;

            x = 0;
            y = Random.Range(1, mazeHeight - 1);

            startZ = 2 * y;
            maze[x, y] = 0;
            Instantiate(startZone, new Vector3(-6f, 0f, startZ), Quaternion.identity);
            edgeList.Add(new int[] { x, y, x, y });

            while (edgeList.Count > 0)
            {
                int index = Random.Range(0, edgeList.Count);
                int[] cell = edgeList[index];
                edgeList.RemoveAt(index);

                x = cell[2];
                y = cell[3];
                if (maze[x, y] == 0)
                {
                    maze[cell[0], cell[1]] = 3;
                    maze[x, y] = 1;

                    if (x >= 3 && maze[x - 2, y] == 0)
                        edgeList.Add(new int[] { x - 1, y, x - 2, y });
                    if (y >= 3 && maze[x, y - 2] == 0)
                        edgeList.Add(new int[] { x, y - 1, x, y - 2 });
                    if (x < mazeWidth - 3 && maze[x + 2, y] == 0)
                        edgeList.Add(new int[] { x + 1, y, x + 2, y });
                    if (y < mazeHeight - 3 && maze[x, y + 2] == 0)
                        edgeList.Add(new int[] { x, y + 1, x, y + 2 });
                }


            }

            bool hasExit = false;
            x = mazeWidth - 3;
            while (!hasExit)
            {
                y = Random.Range(1, mazeHeight - 1);
                if (maze[x, y] == 1)
                {
                    maze[x + 1, y] = 1;
                    maze[x + 2, y] = 1;
                    hasExit = true;
                    Instantiate(endZone, new Vector3(x * 2 + 8f, 0f, y * 2), Quaternion.identity) ;
                }
            }

            for (i = 0; i < mazeHeight; i++)
            {
                for (j = 0; j < mazeWidth; j++)
                {
                    if (maze[i, j] == 1)
                    {
                        Instantiate(space, new Vector3(i * 2, 0, j * 2), Quaternion.identity);

                    }

                    else if (maze[i, j] == 3)
                    {
                        GameObject temp = Instantiate(questionLock, new Vector3(i * 2, 0, j * 2), Quaternion.identity);
                        Instantiate(altSpace, new Vector3(i * 2, 0, j * 2), Quaternion.identity);
                        temp.transform.GetChild(0).GetComponent<QuestionController>().SetQuestionNumber(questionCount);
                        questionCount++;
                    }
                    else
                    {
                        Instantiate(wall, new Vector3(i * 2, 0, j * 2), Quaternion.identity);

                    }

                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
