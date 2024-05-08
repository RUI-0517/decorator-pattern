using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    public abstract int attack { get; }
    public abstract void Use();

    public static void Log(string className, int attack)
    {
        Debug.Log($"{className} �ѱ�ʹ�á�������� {attack} ���˺���");
    }
}