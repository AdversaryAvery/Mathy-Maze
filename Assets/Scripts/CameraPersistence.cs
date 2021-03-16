using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPersistence : MonoBehaviour
{
    public static GameObject exists;
    // Start is called before the first frame update
    void Start()
    {
        if (exists) { Destroy(gameObject); }
        else { exists = gameObject; }
    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
