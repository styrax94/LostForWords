using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddChatOptions : MonoBehaviour
{
    public ScenarioManager scenarioManager;
    public Typewriter typewriter;
    public Narrator narrator;
    public TextMeshProUGUI narratedText;
    public GameObject buttonPrefab;
    public GameObject hintPrefab;
    GameObject[] playerResponse;
    GameObject[] buttonOptions;
    public GameObject endScreen;
    public Transform hintOptionsParent;
    public Transform chatOptionsParent;
    int[] numOfOptions;

    int clickCount = 0;
    int currentScenario = 0;
    bool answerFound = false;
    public GameObject _Continue;




    public void StartChatScenario(int cScenario)
    {
       // Debug.Log(cScenario);
        currentScenario = cScenario;
        if (cScenario <= 19)
        {
            //creates an int and game object array with the size that is according to the player's options in the word puzzles.
            numOfOptions = new int[narrator.multiChatOptions[currentScenario].Length];
            buttonOptions = new GameObject[narrator.multiChatOptions[currentScenario].Length];
        }

        PopulateNarrative(currentScenario);
    }

    public void PopulateOptions()
    {
        if (currentScenario == 21)
        {
            _Continue.SetActive(true);
        }
        if (currentScenario == 20 || currentScenario == 22)
        {
            endScreen.SetActive(true);
        }
        if (currentScenario > 19) return;
        playerResponse = new GameObject[narrator.correctAnswers[currentScenario].Length];

        for (int i = 0; i < playerResponse.Length; i++)
        {
            GameObject prefab = GameObject.Instantiate(hintPrefab, hintOptionsParent);
            prefab.GetComponent<TextMeshPro>().text = "_";
            playerResponse[i] = prefab;
            if (currentScenario == 0 || currentScenario == 19) break;               
        }

        for (int i = 0; i < buttonOptions.Length; i++)
        {
            numOfOptions[i] = i;
            
        }

        for (int i = 0; i < numOfOptions.Length; i++)
        {
            int tempTile = numOfOptions[i];
            int randomInt = Random.Range(i, numOfOptions.Length);
            numOfOptions[i] =  numOfOptions[randomInt];
            numOfOptions[randomInt] = tempTile;
        }

        for (int i = 0; i < buttonOptions.Length; i++)
        {
           GameObject prefab = GameObject.Instantiate(buttonPrefab, chatOptionsParent);
           prefab.GetComponentInChildren<TextMeshProUGUI>().text = narrator.multiChatOptions[currentScenario][numOfOptions[i]];
           prefab.GetComponent<Button>().onClick.AddListener(() => AddAnswer(prefab.GetComponentInChildren<TextMeshProUGUI>().text, prefab, currentScenario));
           buttonOptions[i] = prefab;
        }
    }

    void PopulateNarrative(int currentScenario)
    {
        //Starts displaying the narrative/ scenario by using the typewriter class which uses a coroutine to gradually displays the entire string starting from the first index
        narratedText.text = string.Empty;
        narratedText.alpha = 1;
        typewriter.UseTypewriter(narrator.openingNarration[currentScenario]);
    }

    void AddAnswer(string value, GameObject prefab, int currentScenario)
    {
        //Returns from the method so the player is unable to press more buttons than intended
        if (answerFound || prefab.GetComponent<ButtonPressed>().hasBeenPressed)
        {
            return;
        }
      
      playerResponse[clickCount].GetComponent<TextMeshPro>().text = value;
      playerResponse[clickCount].GetComponent<TextMeshPro>().fontStyle = FontStyles.Underline;
      prefab.GetComponent<ButtonPressed>().ButtonWasPressed();

        if (currentScenario == 0)
        {
            if(value == "He")
            {
                narrator.pronoun = "Dad";
            }
            else if (value == "She")
            {
                narrator.pronoun = "Mum";
            }
            else if(value == "They")
            {
                narrator.pronoun = "Tata";
            }
            narrator.AddPronoun();
            answerFound = true;
            StartCoroutine(Delay(1f,1));
            return;
        }
        else if (currentScenario == 19)
        {
            answerFound = true;

            if (value == "Ring")
            {
                StartCoroutine(Delay(1f, 2));
            }
            else if(value == "Wait")
            {
                StartCoroutine(Delay(1f, 1));
            }
            return;
        }

        //a counter is used to check the sentence the player created with the correct answer
        clickCount++;  
       
      if(clickCount == narrator.correctAnswers[currentScenario].Length)
        {
            CheckAnswers(currentScenario);
        }
    }

    void CheckAnswers(int currentScenario)
    {

        for (int i = 0; i < playerResponse.Length; i++)
        {
            if(playerResponse[i].GetComponent<TextMeshPro>().text != narrator.correctAnswers[currentScenario][i])
            {
                ReEnableButtons();
                clickCount = 0;
                ClearChatChoices();
                return;
            }
        }
        answerFound = true;
        StartCoroutine(Delay(1f,1));
       
    }

    void ReEnableButtons()
    {
        for (int i = 0; i < buttonOptions.Length; i++)
        {
            buttonOptions[i].GetComponent<ButtonPressed>().ButtonReset();
        }
    }

    void ClearChatChoices()
    {
        for (int i = 0; i < playerResponse.Length; i++)
        {
            playerResponse[i].GetComponent<TextMeshPro>().text = "_";
            playerResponse[i].GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
        }
    }

    IEnumerator FadeText(int num)
    {
       
        float alpha = 1.0f;
        
        while (alpha >= 0)
        {
            alpha -= Time.deltaTime;
            narratedText.alpha = alpha;
            yield return null;          
        }
        yield return new WaitForSeconds(0.5f);

       
        clickCount = 0;
        scenarioManager.IncreaseScenario(num);
        answerFound = false;
    }

    

    IEnumerator Delay(float num, int scenarioNum)
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(num);

        for (int i = 0; i < playerResponse.Length; i++)
        {
            Destroy(playerResponse[i]);
        }
        for (int i = 0; i < buttonOptions.Length; i++)
        {
            Destroy(buttonOptions[i]);
        }
        StartCoroutine(FadeText(scenarioNum));
    }

    public void EmptyNarrationText()
    {
        narratedText.text = string.Empty;
    }
}
