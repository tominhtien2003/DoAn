using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Loading
    }

    public GameState CurrentGameState { get; private set; }

    public event Action<int> OnCoinsChanged;

    private const string COINS_KEY = "PlayerCoins";
    private int currentCoins;

    public int CurrentCoins 
    { 
        get => currentCoins;
        private set
        {
            currentCoins = value;
            PlayerPrefs.SetInt(COINS_KEY, currentCoins);
            PlayerPrefs.Save();
            OnCoinsChanged?.Invoke(currentCoins);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentCoins = PlayerPrefs.GetInt(COINS_KEY, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayerStats.Instance.RestoreDefaultStats();
        //GameManager.Instance.ResetCoins();
        //UIMapCreate.ResetMapProgress();
        SetGameState(GameState.MainMenu);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Map"))
        {
            SetGameState(GameState.Playing);
        }
        else
        {
            SetGameState(GameState.MainMenu);
        }
    }

    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;
        
        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenuState();
                break;
            case GameState.Playing:
                HandlePlayingState();
                break;
            case GameState.Paused:
                HandlePausedState();
                break;
            case GameState.GameOver:
                HandleGameOverState();
                break;
            case GameState.Loading:
                HandleLoadingState();
                break;
        }
    }

    private void HandleMainMenuState()
    {
        Time.timeScale = 0f;
    }

    private void HandlePlayingState()
    {
        Time.timeScale = 1f;
    }

    private void HandlePausedState()
    {
        Time.timeScale = 0f;
    }

    private void HandleGameOverState()
    {
        Time.timeScale = 0f;
    }

    private void HandleLoadingState()
    {
        Time.timeScale = 0f; 
    }

    public void StartGame()
    {
        SetGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        SetGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        SetGameState(GameState.Playing);
    }

    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }

    public void ReturnToMainMenu()
    {
        SetGameState(GameState.MainMenu);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        CurrentCoins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || amount > CurrentCoins) return false;
        
        CurrentCoins -= amount;
        Debug.Log($"Spent {amount} coins. Remaining: {CurrentCoins}");
        return true;
    }

    public void ResetCoins()
    {
        CurrentCoins = 0;
        PlayerPrefs.DeleteKey(COINS_KEY);
        PlayerPrefs.Save();
        Debug.Log("Coins reset to 0");
    }
} 