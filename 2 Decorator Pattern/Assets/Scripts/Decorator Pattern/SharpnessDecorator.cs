public class SharpnessDecorator : WeaponDecorator
{
    public override int attack => weapon.attack + 1;
    
    public SharpnessDecorator(IWeapon weapon) : base(weapon)
    {
    }

    public override void Use()
    {
        Weapon.Log(GetType().Name, attack);
    }

}