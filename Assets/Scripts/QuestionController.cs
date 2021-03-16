using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionController : MonoBehaviour
{
    public int questionNumber = 0;
    public string question;
    public int answer;
    public int numberOfTerms = 3;
    private Term term;

    private string questionString;

    // Start is called before the first frame update
    void Start()
    {

        answer = Random.Range(0, 100);
        term = new Term(answer);
        
        for (int count = 1; count < numberOfTerms; count++) {
            term.MakeTerm(answer);
        }

        questionString = term.GetString();
        this.question = "Question " + questionNumber.ToString() + ":\n" + questionString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQuestionNumber(int i) {
        questionNumber = i;
        this.question = "Question " + questionNumber.ToString() + ":\n" + questionString;
    }

    public List<int> GetFactors(int number)
    {
        List<int> factorList = new List<int>();
        if (number == 1) return factorList;
        if (number == 2) return factorList;
   

        for (int i = 2; i < number; i++)
        { // Advance from two to include correct calculation for '4'
            if (number % i == 0) factorList.Add(i);
        }

        return factorList;

    }
}

public class Term
{
    private int number1, number2;

    private Term term1, term2;
    private bool hasTerm1, hasTerm2;
    private bool isMade = false;

    private char sign; // +, -, /, *,

    public string GetString() {
        string tempString;

        if (hasTerm1 && hasTerm2)
        {
            tempString = term1.GetString() + " " + sign + " " + term2.GetString();
        }

        else if (hasTerm1 && !hasTerm2)
        {
            tempString = term1.GetString() + " " + sign + " " + number2.ToString();
        }
        else if (!hasTerm1 && hasTerm2)
        {
            tempString = number1.ToString() + " " + sign + " " + term2.GetString();
        }
        else {
            tempString = number1.ToString() + " " + sign + " " + number2.ToString();
        }

        tempString = "(" + tempString + ")";

        return tempString;
    }

    public void MakeTerm(int value) {

        if (!isMade)
        {
            List<int> factorList = GetFactors(value);
            int temp;
            if (factorList.Count > 0) { temp = Random.Range(0, 4); }
            else { temp = Random.Range(0, 3); }

            switch (temp)
            {
                case 0:
                    sign = '+';
                    number2 = Random.Range(1, value);
                    number1 = value - number2;
                    break;
                case 1:
                    sign = '-';
                    number2 = Random.Range(1, value);
                    number1 = value + number2;
                    break;
                case 2:
                    sign = '/';
                    number2 = Random.Range(1, 11);
                    number1 = value * number2;
                    break;
                case 3:
                    sign = '*';
                    number2 = factorList[Random.Range(0, factorList.Count)];
                    number1 = value / number2;
                    break;
            }
            isMade = true;
        }

        else {
            int choice = Random.Range(0, 2);

            if(choice == 0)
            {
                if (hasTerm1)
                {
                    term1.MakeTerm(number1);
                }
                else
                {
                    term1 = new Term(number1);
                    hasTerm1 = true;
                }
            }
            else
            {
                if (hasTerm2)
                {
                    term2.MakeTerm(number2);
                }
                else
                {
                    term2 = new Term(number2);
                    hasTerm2 = true;
                }
            }
        }
   }
    public Term(int value) {
        this.hasTerm1 = false;
        this.hasTerm2 = false;
        MakeTerm(value);
        }

    public List<int> GetFactors(int number)
    {
        List<int> factorList = new List<int>();
        if (number == 1) return factorList;
        if (number == 2) return factorList;


        for (int i = 2; i < number; i++)
        { // Advance from two to include correct calculation for '4'
            if (number % i == 0) factorList.Add(i);
        }

        return factorList;

    }
}
