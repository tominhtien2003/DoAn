using System;
using UnityEngine;

public class KnightAttackZone : MonoBehaviour
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
            isPlayerInZone = true;
            OnPlayerZoneChanged?.Invoke(this, new PlayerZoneChangedEventArgs { IsPlayerInZone = true });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bool isPlayer = ((1 << collision.gameObject.layer) & playerMask) != 0;

        if (isPlayer)
        {
            isPlayerInZone = false;
            OnPlayerZoneChanged?.Invoke(this, new PlayerZoneChangedEventArgs { IsPlayerInZone = false });
        }
    }
}
