using UnityEngine;

public class BrinerofDeathTeleportState : IEnemyState
{
    public void Enter()
    {
        Debug.Log("Entering Teleport State");
    }

    public void Execute()
    {
        Debug.Log("Executing Teleport State");
    }

    public void Exit()
    {
        Debug.Log("Exiting Teleport State");
    }
}
