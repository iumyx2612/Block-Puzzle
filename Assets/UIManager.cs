using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using DG.Tweening;
using myengine.BlockPuzzle;

public class UIManager : MonoBehaviour
{
    public GameEvent gameOverListen;
    public GameObject gameOverPanel;

    private void OnEnable()
    {
        gameOverListen.AddListener(GameOver);
    }

    private void OnDisable()
    {
        
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }    
}
