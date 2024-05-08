using UnityEngine;

public class Axe : Weapon
{
    public override int attack => 3;

    public override void Use()
    {
        Debug.Log($"{GetType().Name} 已被使用。它造成了 {attack} 点伤害。");
    }
}