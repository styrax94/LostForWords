using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Typewriter : MonoBehaviour
{
    public TextMeshProUGUI narration;
    public AddChatOptions chatOptions;
 
    //uses a coroutine to gradually display a given string
    public void  UseTypewriter(string story)
    {
        StartCoroutine(StartTypewriter(story));
    }
    IEnumerator StartTypewriter(string story)
    {
        string currentN = string.Empty;
        GetComponent<AudioSource>().Play();
        for (int i = 0; i < story.Length; i++)
        {
            currentN += story[i];
            narration.text = currentN;
            if (Input.GetKey(KeyCode.Space))
            {
                currentN = story;
                narration.text = currentN;
                break;
            }
            yield return new WaitForSeconds(0.05f);

           
        }
        GetComponent<AudioSource>().Stop();
        chatOptions.PopulateOptions();
    }
}
