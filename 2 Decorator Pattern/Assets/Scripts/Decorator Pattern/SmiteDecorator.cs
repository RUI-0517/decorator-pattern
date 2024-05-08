public class SmiteDecorator : WeaponDecorator
{
    private string monsterType = "spider";

    public override int attack => monsterType == "skeleton" ? weapon.attack + 1 : weapon.attack;

    public SmiteDecorator(IWeapon weapon) : base(weapon)
    {
    }

    public override void Use()
    {
        string inputMonsterType = "skeleton";
        monsterType = inputMonsterType;

        Weapon.Log(GetType().Name, attack);
    }
}