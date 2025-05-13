using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIMapCreate : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab; 
    [SerializeField] private int numberOfMaps = 5;
    [SerializeField] private string mapScenePrefix = "Map "; // Tiền tố của tên scene map

    private void Start()
    {
        CreateMapButtons();
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

            TextMeshProUGUI buttonTextTMP = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonTextTMP != null)
            {
                buttonTextTMP.text = $"Map {i}";
            }
            int mapIndex = i; 
            buttonObj.GetComponent<Button>().onClick.AddListener(() => OnMapButtonClicked(mapIndex));
        }
    }

    private void OnMapButtonClicked(int mapIndex)
    {
        //Debug.Log($"Đang load Map {mapIndex}");
        string sceneName = $"{mapScenePrefix}{mapIndex}";
        
        // Kiểm tra xem scene có tồn tại trong Build Settings không
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