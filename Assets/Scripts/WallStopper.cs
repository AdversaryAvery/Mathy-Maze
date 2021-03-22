using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStopper : MonoBehaviour
{
    // Start is called before the first frame update

    private bool isBlocked;
    private bool isStarted = false;
    void Start()
    {
        isBlocked = false;
    }

    private void Update()
    {
        if(!isStarted && !Blocker.exists)
        {
            isStarted = true;
            isBlocked = false;
        }
    }

    // Update is called once per frame

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lock") || other.CompareTag("Wall"))
        {
            isBlocked = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lock")|| other.CompareTag("Wall"))
        {
            isBlocked = false;
        }
    }

    public bool GetBlocked() {
        return isBlocked;
    }

    public void ClearBlocked() {
        isBlocked = false;
    }
}
