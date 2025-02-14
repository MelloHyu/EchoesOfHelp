using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int dynamiteKeys = 0;
    public static GameManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void IncreaseKeyByOne()
    {
        dynamiteKeys += 1;
    }

    public void DecreaseKeyByOne()
    {
        if(dynamiteKeys>0)
        {
            dynamiteKeys -= 1;
        }
        
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }



}
