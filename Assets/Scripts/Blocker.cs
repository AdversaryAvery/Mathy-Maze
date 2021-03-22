using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{

    public static bool exists = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isStarted)
        {
            exists = false;
            transform.position += Vector3.right;
            gameObject.SetActive(false);
        }
    }
}
