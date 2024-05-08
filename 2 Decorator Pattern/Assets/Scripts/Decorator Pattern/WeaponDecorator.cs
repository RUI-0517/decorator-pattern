public abstract class WeaponDecorator : IWeapon
{
    protected IWeapon weapon;

    public abstract int attack { get; }
    public abstract void Use();

    protected WeaponDecorator(IWeapon weapon)
    {
        this.weapon = weapon;
    }
}