using UnityEngine;

public class SurikenTrap : IMovingTrap
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            ApplyDamageTo(collision.gameObject);
        }
    }
}
