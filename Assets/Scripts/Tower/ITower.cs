using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    NONE = 0,
    MACHINE_GUN,
    CANNON,
    END,
}

public interface ITower
{
    TowerType GetTowerType();
    bool IsGrounded();
    void SetGrounded(bool value);
}
