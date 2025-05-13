using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class HandleNextMap : MonoBehaviour
{
    [SerializeField] private GameObject panelNextMap;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            panelNextMap.SetActive(true);
        }
    }
    public void OnAsk()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Loading);

        string currentSceneName = SceneManager.GetActiveScene().name;

        Match match = Regex.Match(currentSceneName, @"\d+");
        if (match.Success)
        {
            int currentMapNumber = int.Parse(match.Value);
            int nextMapNumber = currentMapNumber + 1;
            string nextSceneName = "Map " + nextMapNumber;

            if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("Scene " + nextSceneName + " không tồn tại trong Build Settings!");
            }
        }
        else
        {
            Debug.LogError("Tên scene hiện tại không chứa số! Không thể chuyển tiếp map.");
        }
    }
}
