```csharp
class Program
{
    static void Main()
    {
        // Use Case #1: 附魔单一属性
        IWeapon weapon = new Sword();
        IWeapon newWeapon = MakeEnchantedWeapon<SharpnessDecorator>(weapon);
        newWeapon.Use();

        // Use Case #2: 附魔多个属性
        IWeapon weapon = new Sword();
        IWeapon newWeapon = MakeEnchantedWeapon(weapon, typeof(SharpnessDecorator),
                                                        typeof(SmiteDecorator))
        newWeapon.Use();
    }

    static IWeapon MakeEnchantedWeapon<TWeaponDecorator>(IWeapon weapon){
        weapon = new TWeaponDecorator(weapon);
        return newWeapon;
    }

    static IWeapon MakeEnchantedWeapon(IWeapon weapon, params Type[] decorators)
    {
        IWeapon newWeapon = weapon;
        foreach (Type decorator in decorators){
            // Decorate current weapon
            newWeapon = new Type(weapon);
        }
        return newWeapon;
    }
}
```
