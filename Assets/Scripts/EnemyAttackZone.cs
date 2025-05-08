using System;
using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    public event EventHandler<PlayerZoneChangedEventArgs> OnPlayerZoneChanged;

    [SerializeField] private LayerMask playerMask;

    public bool isPlayerInZone { get; private set; } = false;

    public class PlayerZoneChangedEventArgs : EventArgs
    {
        public bool IsPlayerInZone;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayer = ((1 << collision.gameObject.layer) & playerMask) != 0;

        if (isPlayer)
        {
            //Debug.Log("Player entered attack zone");
            isPlayerInZone = true;
            OnPlayerZoneChanged?.Invoke(this, new PlayerZoneChangedEventArgs { IsPlayerInZone = true });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bool isPlayer = ((1 << collision.gameObject.layer) & playerMask) != 0;

        if (isPlayer)
        {
            //Debug.Log("Player exited attack zone");
            isPlayerInZone = false;
            OnPlayerZoneChanged?.Invoke(this, new PlayerZoneChangedEventArgs { IsPlayerInZone = false });
        }
    }
}
