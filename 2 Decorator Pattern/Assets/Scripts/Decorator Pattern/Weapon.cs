using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    public abstract int attack { get; }
    public abstract void Use();

    public static void Log(string className, int attack)
    {
        Debug.Log($"{className} 已被使用。它造成了 {attack} 点伤害。");
    }
}