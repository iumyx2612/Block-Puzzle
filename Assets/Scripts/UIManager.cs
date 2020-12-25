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

    public Text rotateCounterTxt;
    public BoolVariable enableRotate;
    public IntVariable rotateCounter;
    public GameEvent rotate;

    public GameEvent welldoneListen;
    public GameObject welldonePanel;

    private void OnEnable()
    {
        enableRotate.Value = false;
        gameOverListen.AddListener(GameOver);
        rotate.AddListener(RotateCounter);
        welldoneListen.AddListener(WellDone);
    }

    private void OnDisable()
    {
        gameOverListen.RemoveListener(GameOver);
        rotate.RemoveListener(RotateCounter);
        welldoneListen.RemoveListener(WellDone);
        rotateCounter.Value = 3;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }    

    public void WellDone()
    {
        welldonePanel.SetActive(true);
        StartCoroutine(welldonePanelOff());
    }

    IEnumerator welldonePanelOff()
    {
        yield return new WaitForSeconds(2f);
        welldonePanel.SetActive(false);
    }

    //Khi replay k nên load lại Scene vì rất tốn tài nguyên -> Set lại toàn bộ thông tin và deact những thứ cần deact
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RotateCounter()
    {
        rotateCounterTxt.text = rotateCounter.Value.ToString();
    }
}
