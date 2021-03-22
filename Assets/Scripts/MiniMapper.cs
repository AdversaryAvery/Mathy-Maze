using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapper : MonoBehaviour
{

    private int[,] fullMaze;
    private int[,] seenMaze;
    private int mazeSize;

    private int xoffset = 40;
    private int yoffset = -64;

    public GameObject whiteTile;
    public GameObject blackTile;
    public GameObject redTile;

    public GameObject player;

    public int posX = -3;
    public int posY = 0;

    public GameObject[,] minMap;


    // Start is called before the first frame update
    void Start()
    {

        

        fullMaze = GameController.maze;

        mazeSize = GameController.mazeHeight;

        seenMaze = new int[mazeSize, mazeSize];
        minMap = new GameObject[mazeSize, mazeSize];

        for (int i = 0; i < mazeSize; i++) {
            for (int j = 0; j < mazeSize; j++)
            {
                seenMaze[i, j] = 0;
                
                if (fullMaze[i, j] == 1 || fullMaze[i, j] == 3) {
                    minMap[j, i] = Instantiate(whiteTile, new Vector3(-j * 8 + xoffset, i * 8 + yoffset, 0), Quaternion.identity);
                    minMap[j, i].transform.SetParent(transform, false);
                    
                }
                else
                {
                    minMap[j, i] = Instantiate(blackTile, new Vector3(-j * 8 + xoffset, i * 8 + yoffset, 0), Quaternion.identity);
                    minMap[j, i].transform.SetParent(transform, false);
                }

                minMap[j, i].SetActive(false);
            }
        }
        redTile = Instantiate(redTile, Vector3.zero, Quaternion.identity);
        redTile.transform.SetParent(transform, false);

        posY = GameController.startZ / 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        

        if (ControlScript.pos1 >0 && ControlScript.pos2 > 0 && ControlScript.pos1 < mazeSize-1 && ControlScript.pos2 < mazeSize - 1)
            {
            redTile.transform.position = minMap[ControlScript.pos2, ControlScript.pos1].transform.position;
            redTile.SetActive(true);
            for (int i = ControlScript.pos2 - 1; i < ControlScript.pos2 + 2; i++)
            {
                for (int j = ControlScript.pos1 - 1; j < ControlScript.pos1 + 2; j++)
                {
                    if(seenMaze[i,j] == 0)
                    {
                        minMap[i, j].SetActive(true);

                        seenMaze[i, j] = 1;
                    }
                }
            }
        }
        else
        {
            redTile.SetActive(false);
        }
    }

 }
