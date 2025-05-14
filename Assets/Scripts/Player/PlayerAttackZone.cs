using System;
using UnityEngine;

public class PlayerAttackZone : MonoBehaviour
{
    public event EventHandler<EnemyZoneChangedEventArgs> OnEnemyZoneChanged;

    [SerializeField] private LayerMask enemyMask;

    public bool isEnemyInZone { get; private set; } = false;

    public class EnemyZoneChangedEventArgs : EventArgs
    {
        public bool IsEnemyInZone;
        public GameObject EnemyObject; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isEnemy = ((1 << collision.gameObject.layer) & enemyMask) != 0;

        if (isEnemy)
        {
            isEnemyInZone = true;
            OnEnemyZoneChanged?.Invoke(this, new EnemyZoneChangedEventArgs
            {
                IsEnemyInZone = true,
                EnemyObject = collision.gameObject
            });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bool isEnemy = ((1 << collision.gameObject.layer) & enemyMask) != 0;

        if (isEnemy)
        {
            isEnemyInZone = false;
            OnEnemyZoneChanged?.Invoke(this, new EnemyZoneChangedEventArgs
            {
                IsEnemyInZone = false,
                EnemyObject = null
            });
        }
    }
}
