using UnityEngine;

public class UIButtonMap : MonoBehaviour
{
    [SerializeField] private GameObject uiLockObj;
    public bool isLocked;
    public bool IsLocked
    {
        get { return isLocked; }
        set
        {
            if (value == isLocked) return; 
            isLocked = value;
            OnUpdateUILock();
        }
    }
    private void OnUpdateUILock()
    {
        if (uiLockObj != null)
        {
            uiLockObj.SetActive(!uiLockObj.activeSelf);
        }
        else
        {
            Debug.LogError("uiLockObj is not assigned in the inspector.");
        }
    }
}
