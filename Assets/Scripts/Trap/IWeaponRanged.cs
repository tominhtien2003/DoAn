using UnityEngine;

public interface IWeaponRanged
{
    void Initialize(Vector3 position, float direction);
    void Deactivate();
}
