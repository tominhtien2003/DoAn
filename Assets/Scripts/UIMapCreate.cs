using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIMapCreate : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab; 
    [SerializeField] private int numberOfMaps;
    [SerializeField] private string mapScenePrefix = "Map "; 

    private const string UNLOCKED_MAPS_KEY = "UnlockedMaps";

    private void Start()
    {
        CreateMapButtons();
    }

    private bool IsMapUnlocked(int mapNumber)
    {
        if (mapNumber == 1) return true;

        string unlockedMaps = PlayerPrefs.GetString(UNLOCKED_MAPS_KEY, "1");
        string[] unlockedMapNumbers = unlockedMaps.Split(',');
        
        foreach (string mapNum in unlockedMapNumbers)
        {
            if (int.TryParse(mapNum, out int num) && num == mapNumber)
            {
                return true;
            }
        }
        return false;
    }

    public static void UnlockMap(int mapNumber)
    {
        string unlockedMaps = PlayerPrefs.GetString(UNLOCKED_MAPS_KEY, "1");
        string[] unlockedMapNumbers = unlockedMaps.Split(',');

        foreach (string mapNum in unlockedMapNumbers)
        {
            if (int.TryParse(mapNum, out int num) && num == mapNumber)
            {
                return;
            }
        }

        unlockedMaps = unlockedMaps + "," + mapNumber;
        PlayerPrefs.SetString(UNLOCKED_MAPS_KEY, unlockedMaps);
        PlayerPrefs.Save();
    }

    public static void ResetMapProgress()
    {
        PlayerPrefs.SetString(UNLOCKED_MAPS_KEY, "1");
        PlayerPrefs.Save();        
    }

    public void CreateMapButtons()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= numberOfMaps; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, transform);
            buttonObj.name = $"MapButton_{i}";

            UIButtonMap buttonMap = buttonObj.GetComponent<UIButtonMap>();

            if (buttonMap != null)
            {
                buttonMap.IsLocked = !IsMapUnlocked(i);
            }
            else
            {
                Debug.LogError("UIButtonMap component not found on the button prefab.");
            }
            TextMeshProUGUI buttonTextTMP = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonTextTMP != null)
            {
                buttonTextTMP.text = $"Map {i}";
            }
            int mapIndex = i; 
            buttonObj.GetComponent<Button>().onClick.AddListener(() => OnMapButtonClicked(buttonMap, mapIndex));
        }
    }

    private void OnMapButtonClicked(UIButtonMap uIButtonMap, int mapIndex)
    {
        if (uIButtonMap.IsLocked)
        {
            return;
        }
        string sceneName = $"{mapScenePrefix}{mapIndex}";
        
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) != -1)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Loading);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene {sceneName} không tồn tại trong Build Settings!");
        }
    }
} 