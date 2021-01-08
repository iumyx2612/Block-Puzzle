using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using DG.Tweening;
using myengine.BlockPuzzle;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObjectCollection remainBlocks;
    public GameEvent gameOverListen;
    public GameObject gameOverPanel;
    [SerializeField] private GameEvent shopNeed;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Text gameOverCurScore;
    [SerializeField] private Text gameOverBestScore;
    [SerializeField] private IntVariable curScore;
    [SerializeField] private IntVariable bestScore;

    public Text rotateCounterTxt;
    public BoolVariable enableRotate;
    public IntVariable rotateCounter;
    public GameEvent rotate;
    [SerializeField] private GameEvent rotateBtnClick;

    [SerializeField] private GameEvent replayListen;

    [SerializeField] private GameEvent welldoneListen;
    [SerializeField] private GameObject welldonePanel;

    [SerializeField] private GameEvent pauseMenuListen;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject musicBtn;
    [SerializeField] private GameEvent musicBtnListen;
    [SerializeField] private Sprite[] musicBtnImages;

    [SerializeField] private GameObject sfxBtn;
    [SerializeField] private GameEvent sfxBtnListen;

    [SerializeField] private GameObject vibrateBtn;
    [SerializeField] private GameEvent vibrateBtnListen;
    [SerializeField] private Sprite[] vibrateBtnImages;

    [SerializeField] private GameObject havingFunPanel;

    [SerializeField] private GameEvent closeBtnClick;

    [SerializeField] private GameEvent btnClick;

    [SerializeField] private IntGameEvent rotateBuy;

    private void OnEnable()
    {
        enableRotate.Value = false;
        gameOverListen.AddListener(GameOver);
        shopNeed.AddListener(ShopNeed);
        rotate.AddListener(RotateCounter);
        welldoneListen.AddListener(WellDone);
        pauseMenuListen.AddListener(PauseMenu);
        musicBtnListen.AddListener(MusicBtn);
        sfxBtnListen.AddListener(SfxBtn);
        vibrateBtnListen.AddListener(VibrateBtn);
        replayListen.AddListener(Replay);
        rotateBuy.AddListener(RotateBuy);

        closeBtnClick.AddListener(CloseBtn);
        btnClick.AddListener(BtnClick);
        Time.timeScale = 1f;
        rotateCounterTxt.text = rotateCounter.Value.ToString();
    }

    private void OnDisable()
    {
        gameOverListen.RemoveListener(GameOver);
        shopNeed.RemoveListener(ShopNeed);
        rotate.RemoveListener(RotateCounter);
        welldoneListen.RemoveListener(WellDone);
        pauseMenuListen.RemoveListener(PauseMenu);
        musicBtnListen.RemoveListener(MusicBtn);
        sfxBtnListen.RemoveListener(SfxBtn);
        vibrateBtnListen.RemoveListener(VibrateBtn);
        replayListen.RemoveListener(Replay);
        rotateBuy.RemoveListener(RotateBuy);

        closeBtnClick.RemoveListener(CloseBtn);
        btnClick.RemoveListener(BtnClick);
        //rotateCounter.Value = 3;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverCurScore.text = curScore.Value.ToString();
        gameOverBestScore.text = bestScore.Value.ToString();
        Time.timeScale = 0f;
    }    

    public void ShopNeed()
    {
        shopPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseBtn()
    {
        Time.timeScale = 1f;
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

    public void PauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void MusicBtn()
    {        
        if (musicBtn.transform.GetChild(0).GetComponent<Image>().sprite != musicBtnImages[0])
        {
            musicBtn.transform.GetChild(0).GetComponent<Image>().sprite = musicBtnImages[0];
            AudioManager._instance.UnMute("Theme");
        }
        else
        {
            musicBtn.transform.GetChild(0).GetComponent<Image>().sprite = musicBtnImages[1];
            AudioManager._instance.Mute("Theme");
        }
    }

    public void SfxBtn()
    {
        sfxBtn.transform.GetChild(0).gameObject.SetActive(!sfxBtn.transform.GetChild(0).gameObject.activeSelf);
        if(sfxBtn.transform.GetChild(0).gameObject.activeSelf)
        {
            AudioManager._instance.Mute("One");
            AudioManager._instance.Mute("Lose");
            AudioManager._instance.Mute("Place");
            AudioManager._instance.Mute("UIClick");
            AudioManager._instance.Mute("Multiple");
        }
        else
        {
            AudioManager._instance.UnMute("One");
            AudioManager._instance.UnMute("Lose");
            AudioManager._instance.UnMute("Place");
            AudioManager._instance.UnMute("UIClick");
            AudioManager._instance.UnMute("Multiple");
        }
    }

    public void VibrateBtn()
    {
        if (vibrateBtn.transform.GetChild(0).gameObject.activeSelf)
        {
            vibrateBtn.transform.GetChild(0).gameObject.SetActive(false);
            vibrateBtn.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            vibrateBtn.transform.GetChild(0).gameObject.SetActive(true);
            vibrateBtn.transform.GetChild(1).gameObject.SetActive(false);
        }
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

    public void RotateBuy(int counter)
    {
        rotateCounter.Value += counter;
        RotateCounter();
    }

    public void Toggle()
    {
        if(rotateCounter.Value <= 0)
        {
            ShopNeed();
            return;
        }
        enableRotate.Value = !enableRotate.Value;
        rotateBtnClick.Raise();
    }

    public void BtnClick()
    {
        AudioManager._instance.Play("UIClick");
    }
}
