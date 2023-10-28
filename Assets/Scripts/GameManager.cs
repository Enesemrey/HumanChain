using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Statics
    public const float version = 0.012f;
    public const int firstSceneIndex = 1;
    public const int tutorialIndex = 1;
    public const string TutorialHash = "TutorialPlayed";
    public const string LevelIndexHash = "LevelIndex";
    public const string LevelHash = "Level";
    private static GameManager _instance;
    public static GameManager Instance =>
        _instance != null
            ? _instance
            : _instance = FindObjectOfType<GameManager>();

    //Unity Events
    public UnityEvent GameStarted;
    public UnityEvent OnLevelFail;
    public UnityEvent OnLevelWin;

    //Game Flags
    public bool IsPlaying;
    public bool IsGameOver;

    public CinemachineVirtualCamera EndCam;

    private bool _tutorialPlayed
    {
        get => PlayerPrefs.GetInt(TutorialHash + version, 0) == 1;
        set => PlayerPrefs.SetInt(TutorialHash + version, value ? 1 : 0);
    }
    private int _currentLevelIndex
    {
        get => PlayerPrefs.GetInt(LevelIndexHash + version, _tutorialPlayed ? tutorialIndex : firstSceneIndex);
        set => PlayerPrefs.SetInt(LevelIndexHash + version, value);
    }

    public int CurrentLevel
    {
        get => PlayerPrefs.GetInt(LevelHash + version, 1);
        set => PlayerPrefs.SetInt(LevelHash + version, value);
    }

    public int FinishCount;


    // Start is called before the first frame update
    void Awake()
    {
        FinishCount = 0;
        Debug.Log("Index: " + _currentLevelIndex);
        Debug.Log("Level: " + CurrentLevel);
#if !UNITY_EDITOR
        if (_currentLevelIndex != SceneManager.GetActiveScene().buildIndex)
            SceneManager.LoadScene(_currentLevelIndex);
#endif


        _instance = this;
        if (_currentLevelIndex == tutorialIndex)
            _tutorialPlayed = true;

        Analytics.LevelStarted(_currentLevelIndex);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (!IsPlaying && !IsGameOver)
    //    {
    //        if (Input.anyKeyDown)
    //        {
    //            IsPlaying = true;
    //            GameStarted.Invoke();
    //        }
    //        return;
    //    }
    //}
    IEnumerator Start()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        IsPlaying = true;
        GameStarted.Invoke();
        var glasses = GameObject.FindObjectsOfType<BreakGlass>();
        yield return new WaitUntil(() => glasses.All(m => m.IsBroken));
        //if (!IsPlaying) yield break;
        //Win();
    }


    public void Fail()
    {
        IsPlaying = false;
        IsGameOver = true;
        Analytics.LevelFailed(_currentLevelIndex);
        OnLevelFail.Invoke();
    }
    public void Win()
    {
        VCamManager.Instance.SetCam(EndCam);
        IsPlaying = false;
        IsGameOver = true;
        UIManager.Instance.SetFinishText(FinishCount);
        Analytics.LevelPassed(_currentLevelIndex);
        StartCoroutine(DelayWin(2));
    }

    private IEnumerator DelayWin(int duration)
    {
        yield return new WaitForSeconds(duration);
        OnLevelWin.Invoke();

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        CurrentLevel++;
        var isRandomLevel = CurrentLevel > SceneManager.sceneCountInBuildSettings - firstSceneIndex;
        if (isRandomLevel)
        {
            _currentLevelIndex = Random.Range(firstSceneIndex, SceneManager.sceneCountInBuildSettings);
        }
        else
        {
            _currentLevelIndex++;
        }
        Debug.Log("Next Index: " + _currentLevelIndex);
        Debug.Log("Next Level: " + CurrentLevel);
        SceneManager.LoadScene(_currentLevelIndex);
    }

}
