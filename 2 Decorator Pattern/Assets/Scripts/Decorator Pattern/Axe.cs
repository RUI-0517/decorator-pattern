using UnityEngine;

public class Axe : Weapon
{
    public override int attack => 3;

    public override void Use()
    {
        Debug.Log($"{GetType().Name} �ѱ�ʹ�á�������� {attack} ���˺���");
    }
}