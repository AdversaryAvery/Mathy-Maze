using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyController : MonoBehaviour
{

    public GameObject questionUI;
    public TextMeshProUGUI questionText;
    public ControlScript controlScript;
    private GameObject questionLock;
    public TMP_InputField inputField;
    public AudioSource goodBlip;
    public AudioSource badBlip;
    public int answer;


    // Start is called before the first frame update
    void Start()
    {
        questionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lock")) 
        {
            QuestionController questionController = other.GetComponent<QuestionController>();
            questionLock = questionController.gameObject.transform.parent.gameObject;
            questionUI.SetActive(true);
            questionText.text = questionController.question;
            answer = questionController.answer;
            inputField.ActivateInputField();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lock"))
        {
            questionUI.SetActive(false);
        }
    }

    public void SubmitAnswer()
    {
        if (int.Parse(inputField.text) == answer)
        {
            Destroy(questionLock);
            inputField.text = "";
            questionUI.SetActive(false);
            gameObject.GetComponent<WallStopper>().ClearBlocked();
            goodBlip.Play();
        }

        else {
            inputField.text = "";
            badBlip.Play();
        }
    }
}
