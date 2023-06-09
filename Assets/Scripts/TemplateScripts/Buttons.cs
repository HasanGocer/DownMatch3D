using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Buttons : MonoSingleton<Buttons>
{
    //managerde bulunacak
    [Header("Global_Panel")]
    [Space(10)]

    [SerializeField] private GameObject _globalPanel;
    public TMP_Text moneyText, levelText, nextLevelText;

    [Header("Start_Panel")]
    [Space(10)]

    public GameObject startPanel;
    [SerializeField] Button _startButton;

    [Header("Setting_Panel")]
    [Space(10)]

    [SerializeField] private Button _settingButton;
    [SerializeField] private GameObject _settingGame;

    [SerializeField] private Sprite _red, _green;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _settingBackButton;
    [SerializeField] private Button _soundButton, _vibrationButton;

    [Header("Finish_Panel")]
    [Space(10)]

    public GameObject winPanel;
    public GameObject failPanel, newObjectPanel;
    public GameObject newObjectGO;
    [SerializeField] private Button _winPrizeButton, _winEmptyButton, _failButton, _newObjectButton;
    [SerializeField] int finishWaitTime;

    [Header("Loading_Panel")]
    [Space(10)]

    [SerializeField] GameObject _loadingPanel;
    [SerializeField] Image _loadingLerpBar;
    [SerializeField] int _loadingLerpSpeed;
    [SerializeField] int _startSceneCount;


    private void Start()
    {
        ButtonPlacement();
        SettingPlacement();
        levelText.text = GameManager.Instance.level.ToString();
        nextLevelText.text = (GameManager.Instance.level + 1).ToString();
    }

    public IEnumerator LoadingScreen()
    {
        if (Load.Instance != null)
            if (Load.Instance.isNext)
            {
                yield return null;
                PlacementSystem.Instance.StartPlacement();
                TaskSystem.Instance.FirstStart();

                _loadingPanel.SetActive(false);
                _globalPanel.SetActive(true);
                startPanel.SetActive(false);
                StartButton();
            }
            else
            {
                PlacementSystem.Instance.StartPlacement();
                TaskSystem.Instance.FirstStart();
                _loadingPanel.SetActive(true);
                _globalPanel.SetActive(false);
                startPanel.SetActive(false);

                float lerpCount = 0;

                while (true)
                {
                    lerpCount += Time.deltaTime * _loadingLerpSpeed;
                    _loadingLerpBar.fillAmount = Mathf.Lerp(0, 1, lerpCount);
                    yield return new WaitForSeconds(Time.deltaTime);
                    if (_loadingLerpBar.fillAmount == 1) break;
                }

                _loadingPanel.SetActive(false);
                _globalPanel.SetActive(true);
                startPanel.SetActive(true);

            }
        else
        {
            PlacementSystem.Instance.StartPlacement();
            TaskSystem.Instance.FirstStart();
            _loadingPanel.SetActive(true);
            _globalPanel.SetActive(false);
            startPanel.SetActive(false);

            float lerpCount = 0;

            while (true)
            {
                lerpCount += Time.deltaTime * _loadingLerpSpeed;
                _loadingLerpBar.fillAmount = Mathf.Lerp(0, 1, lerpCount);
                yield return new WaitForSeconds(Time.deltaTime);
                if (_loadingLerpBar.fillAmount == 1) break;
            }

            _loadingPanel.SetActive(false);
            _globalPanel.SetActive(true);
            startPanel.SetActive(true);

        }

    }
    public IEnumerator NoThanxOnActive()
    {
        _winEmptyButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        _winEmptyButton.gameObject.SetActive(true);
    }

    public void SettingPanelOff()
    {
        SettingBackButton();
    }

    private void SettingPlacement()
    {
        SoundSystem soundSystem = SoundSystem.Instance;
        GameManager gameManager = GameManager.Instance;
        Image soundImage = _soundButton.gameObject.GetComponent<Image>();
        Image vibrationImage = _vibrationButton.gameObject.GetComponent<Image>();

        if (gameManager.sound == 1)
        {
            soundImage.sprite = _green;
            soundSystem.MainMusicPlay();
        }
        else
        {
            soundImage.sprite = _red;
            soundSystem.MainMusicStop();
        }

        if (gameManager.vibration == 1)
            vibrationImage.sprite = _green;
        else
            vibrationImage.sprite = _red;
    }
    private void ButtonPlacement()
    {
        _settingButton.onClick.AddListener(SettingButton);
        _settingBackButton.onClick.AddListener(SettingBackButton);
        _soundButton.onClick.AddListener(SoundButton);
        _vibrationButton.onClick.AddListener(VibrationButton);
        _winPrizeButton.onClick.AddListener(() => StartCoroutine(WinPrizeButton()));
        _winEmptyButton.onClick.AddListener(() => StartCoroutine(WinButton()));
        _failButton.onClick.AddListener(() => StartCoroutine(FailButton()));
        _startButton.onClick.AddListener(StartButton);
        _homeButton.onClick.AddListener(HomeButton);
        _newObjectButton.onClick.AddListener(NewObjectButton);
    }

    private void NewObjectButton()
    {
        winPanel.SetActive(true);
        newObjectPanel.SetActive(false);
        newObjectGO.SetActive(false);
    }
    private void HomeButton()
    {
        Load.Instance.isNext = false;
        SceneManager.LoadScene(_startSceneCount);
    }
    private void StartButton()
    {

        GameManager.Instance.gameStat = GameManager.GameStat.start;

        startPanel.SetActive(false);

        PlacementSystem.Instance.ObjectPlacement();
        TaskUISystem.Instance.UIPlacement();
        CounterSystem.Instance.CounterStart();
    }
    private IEnumerator WinButton()
    {
        GameManager gameManager = GameManager.Instance;

        _winPrizeButton.enabled = false;
        gameManager.SetLevel();
        BarSystem.Instance.BarStopButton(0);
        MoneySystem.Instance.MoneyTextRevork(gameManager.addedMoney);
        yield return new WaitForSeconds(finishWaitTime);


        gameManager.SetLevel();

        SceneManager.LoadScene(_startSceneCount);
    }
    private IEnumerator WinPrizeButton()
    {
        GameManager gameManager = GameManager.Instance;

        _winPrizeButton.enabled = false;
        yield return new WaitForSeconds(finishWaitTime);


        gameManager.SetLevel();

        SceneManager.LoadScene(_startSceneCount);
    }
    private IEnumerator FailButton()
    {
        Load.Instance.isNext = true;
        MoneySystem.Instance.MoneyTextRevork(GameManager.Instance.addedMoney);
        yield return new WaitForSeconds(finishWaitTime);


        SceneManager.LoadScene(_startSceneCount);
    }
    private void SettingButton()
    {
        if (GameManager.Instance.gameStat != GameManager.GameStat.finish)
        {
            startPanel.SetActive(false);
            _settingGame.SetActive(true);
            _settingButton.gameObject.SetActive(false);
            _globalPanel.SetActive(false);
        }
    }
    private void SettingBackButton()
    {
        if (GameManager.Instance.gameStat == GameManager.GameStat.intro)
            startPanel.SetActive(true);
        _settingGame.SetActive(false);
        _settingButton.gameObject.SetActive(true);
        _globalPanel.SetActive(true);
    }
    private void SoundButton()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager.sound == 1)
        {
            _soundButton.gameObject.GetComponent<Image>().sprite = _red;
            SoundSystem.Instance.MainMusicStop();
            gameManager.sound = 0;
        }
        else
        {
            _soundButton.gameObject.GetComponent<Image>().sprite = _green;
            SoundSystem.Instance.MainMusicPlay();
            gameManager.sound = 1;
        }

        gameManager.SetSound();
    }
    private void VibrationButton()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager.vibration == 1)
        {
            _vibrationButton.gameObject.GetComponent<Image>().sprite = _red;
            gameManager.vibration = 0;
        }
        else
        {
            _vibrationButton.gameObject.GetComponent<Image>().sprite = _green;
            gameManager.vibration = 1;
        }

        gameManager.SetVibration();
    }
    public void NotBossFinal()
    {
        _winPrizeButton.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "Next";
    }
}
