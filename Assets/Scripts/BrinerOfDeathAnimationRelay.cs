using UnityEngine;

public class BrinerOfDeathAnimationRelay : MonoBehaviour
{
    [SerializeField] private BrinerofDeathBoss boss;
    public void OnMeleeAttackAnimationFinished()
    {
        boss?.OnMeleeAttackAnimationFinished();
    }
}
