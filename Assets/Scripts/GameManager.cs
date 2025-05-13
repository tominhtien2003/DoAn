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

    // Sự kiện khi số coin thay đổi
    public event Action<int> OnCoinsChanged;

    private const string COINS_KEY = "PlayerCoins";
    private int currentCoins;

    public int CurrentCoins 
    { 
        get => currentCoins;
        private set
        {
            currentCoins = value;
            // Lưu coin vào PlayerPrefs
            PlayerPrefs.SetInt(COINS_KEY, currentCoins);
            PlayerPrefs.Save();
            // Kích hoạt sự kiện
            OnCoinsChanged?.Invoke(currentCoins);
        }
    }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Load số coin đã lưu
            currentCoins = PlayerPrefs.GetInt(COINS_KEY, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize game state
        SetGameState(GameState.MainMenu);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nếu scene được load là một map (có chứa "Map" trong tên)
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

    // Thêm coin
    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        CurrentCoins += amount;
        Debug.Log($"Added {amount} coins. Total: {CurrentCoins}");
    }

    // Trừ coin (ví dụ: khi mua item)
    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || amount > CurrentCoins) return false;
        
        CurrentCoins -= amount;
        Debug.Log($"Spent {amount} coins. Remaining: {CurrentCoins}");
        return true;
    }

    // Reset coin (ví dụ: khi chơi lại từ đầu)
    public void ResetCoins()
    {
        CurrentCoins = 0;
        Debug.Log("Coins reset to 0");
    }
} 