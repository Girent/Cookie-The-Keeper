using System;
using UnityEngine;

public interface IHealth
{
    public void ApplyDamage(float amount, uint netId);
}
