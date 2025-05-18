using UnityEngine;

public class HandleNextMap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            InventoryManager.Instance.panelNextMap.SetActive(true);
        }
    }

}
