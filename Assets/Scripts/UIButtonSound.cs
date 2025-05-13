using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayButtonSound);
    }
    private void PlayButtonSound()
    {
        AudioManager.Instance.PlaySound("Select");
        if (gameObject.tag == "TurnOnSound")
        {
            AudioManager.Instance.IsTurnOnSound = !AudioManager.Instance.IsTurnOnSound;
        }
        if (gameObject.tag == "TurnOnMusic")
        {
            AudioManager.Instance.IsTurnOnMusic = !AudioManager.Instance.IsTurnOnMusic;
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(PlayButtonSound);
        }
    }
} 