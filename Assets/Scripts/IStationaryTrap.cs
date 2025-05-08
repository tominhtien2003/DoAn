using UnityEngine;

public abstract class IStationaryTrap : BaseTrap
{
    [SerializeField] protected float activationDelay = 0.2f;
    [SerializeField] protected float cooldownTime = 1.5f;

    protected float cooldownTimer = 0f;
    protected bool isActivated = false;

    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f) OnCooldownComplete();
        }
    }

    protected void TriggerTrap()
    {
        if (isActivated || cooldownTimer > 0f) return;

        isActivated = true;
        cooldownTimer = cooldownTime;
        Invoke(nameof(ExecuteTrap), activationDelay);
    }

    protected virtual void ResetTrap()
    {
        isActivated = false;
    }

    protected virtual void OnCooldownComplete() { }

    protected abstract void ExecuteTrap();
}
