using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
   public int currentScenario;
    public AddChatOptions scenario;
    AudioSource[] audioSources;
    int count;
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    public void StartScenario()
    {
        scenario.StartChatScenario(currentScenario);

    }
    public void IncreaseScenario(int num)
    {
        currentScenario += num;
        
        StartScenario();
        if(currentScenario == 7)
        {
            audioSources[count].Stop();
            count++;
            audioSources[count].Play();
        }
        else if(currentScenario == 18)
        {
            audioSources[count].Stop();
            count++;
            audioSources[count].Play();
        }
    }

    public void RestartGame()
    {
        currentScenario = 0;
        
    }
}
