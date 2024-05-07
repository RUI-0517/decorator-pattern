# 装饰者模式

想象你在玩 Minecraft，你想给你的武器附魔。
你可以随意给你的武器附加任何效果，组合的可能性无穷无尽。

但是，程序员是如何编程实现这一功能的呢？非常高兴他们不需要一一编写每种可能性，因为他们使用了“装饰者模式”。

## 什么是装饰者模式

你可以将装饰者模式想象成日常生活中的穿衣服。
你尝试在所有可能性中，添加一件不确定的衣物到你的身上。
你可能会考虑使用很多标志（flag），然后检查一个标志是否有效并执行相应的操作。
显然，如果你有更多的标志要添加，这种方式难以扩展，最终你会得到大量的 `if-else` 或 `switch` 语句。

那么我们如何解决这个问题，或者更简单地，如何描述一个人所穿的衣物的信息/状态？

你可以描述一个人戴着红帽子，然后是衬衫等......这就是理念，你可以想象一个赤裸的人，并在他/她身上添加更多的衣物。因此，装饰者就是穿在赤裸人身上的衣物。

在游戏中，它可以是 Buff(Stat Modifier)，或者强化物品(Power-ups)等。

## 装饰者模式的结构

“很好，我理解了背后的想法，但仍然不知道如何编程。”
回到 Minecraft 的场景。玩家想要给他/她的物品附魔。
如果你从未玩过 Minecraft（这几乎不可能吧？）并且不知道什么是附魔，请看下面的视频。

基本上，玩家可以选择一个物品，然后在其上添加额外效果。例如，`Sharpness`（锋利）使武器造成更多伤害，`Smite`（亡灵杀手）意味着你可以对骷髅等特定怪物造成更多伤害。
在后期版本中，我们有了一种名为 `Trident`（三叉戟）的新武器，它有一个特殊的附魔，允许扔出的三叉戟返回到玩家手中。

因此，我们知道它主要有两个任务：

- 改变物品属性
- 改变物品行为

我们可以根据这两个属性来建模我们的结构。

首先，我们最好有一个接口 `IWeapon` 并确保它可以被使用：

```csharp
interface IWeapon
{
    void Use();
}
```

现在如果我们想改变它的属性或行为。我们可以重载（override）它的属性或 `Use()` 函数！

不同的武器可能具有不同的属性，我们将为 `Weapon` 创建一个 `attack` 属性。

```csharp
abstract class Weapon : IWeapon
{
    public int attack { get; }
    public void Use();
}
```

一切顺利，现在我们可以创建具体的武器，比如 `Axe`，或者任何你喜欢的！

```csharp
class Axe : Weapon
{
    public int attack { get; }

    public override void Use()
    {
        Debug.Log($"{GetType().Name} 已被使用。它造成了 {attack} 点伤害。");
    }
}
```

这里没有什么特别的。现在我们来处理装饰者部分。
为了重写属性或行为，它也需要实现 `IWeapon` 接口。
这种关系在你做卡牌游戏的时候也很有效，一张卡可以被正常打出或强化其他卡牌。

```csharp
abstract class WeaponDecorator : IWeapon
{
    protected IWeapon weapon;
    public void Use();
}
```

我们还添加了一个属性来存储你正在修饰的武器，比如你想为武器增加 1 点伤害，你可以这样做：

```csharp
protected IWeapon weapon;
public attack => weapon.attack + 1;
```

如果你想装饰特定的武器，你最好为其创建一个接口：

```csharp
interface IAxe : IWeapon
{
    int attack { get; }
    void Use();
}

interface ISword : IWeapon
{
    int attack { get; }
    int range {get; }
    void Use();
}
```

这样确保你可以访问并覆盖特定武器的属性，不同的武器具有不同的属性。

然后对于装饰者你可以这样做：

```csharp
class AxeDecorator : IAxe
{
    protected IAxe weapon;

    int attack { get; }
    void Use();
}

class SwordDecorator : ISword
{
    protected ISword weapon;

    int attack { get; }
    int range {get; }
    void Use();
}
```

# 示例：Minecraft 附魔

我们终于到这一步了！我们将进行一个练习，像 Minecraft 那样为武器附魔。
如果你想亲自尝试，你可以从 GitHub 拉取 `starter` 分支。
让我先解释一下场景。

## 第一步：实现武器

首先，我们应该实现我们自己的接口和类。
在 Unity 项目中的 `Scripts/Decorator Pattern` 文件夹里，我已经定义了 `IWeapon` 接口。

```csharp
public interface IWeapon
{
    public int attack { get; }
    public void Use();
}
```

我建议在 `Use()` 函数中仅实现日志打印功能，比如可以这样写：

```csharp
public void Use()
{
    Debug.Log($"{GetType().Name} 已被使用。它造成了 {attack} 点伤害。");
}
```

其次，我们将实现两个通用的装饰者：

- Sharpness（锋利）：当装饰到一件武器上时，它会使武器的攻击力增加 1；
- Smite（亡灵杀手）：当攻击骷髅时，会额外造成 1 点伤害。

由于缺乏上下文，你可以随意硬编码一些逻辑。

**如果你正在使用启动包，你应该这样做：**

1. 将 `Sharpness` 效果命名为 `SharpnessDecorator`
2. 将 `Smite` 效果命名为 `SmiteDecorator`

这是因为在这个场景中，我们通过字符串添加装饰者，所以如果命名不匹配，当你点击按钮时，它会报错：

> 未找到装饰器类型或类型不合法: SharpnessDecorator

> ArgumentException: 提供的附魔类型字符串无效或包含无法识别的类型名称

## 第二步：实现装饰者逻辑

在 `EnchantmentManager.cs` 中，我为你创建了两个函数来实现，`MakeEnchantedWeapon` 和 `IsValidDecoratorType`。

对于 `MakeEnchantedWeapon`，你只需要按照指导操作即可：

```csharp
/// <summary>
/// 对武器进行附魔。此方法将接受一个包含一个或多个附魔类型的字符串输入。
/// 输入字符串中的附魔类型应以分号 (';') 分隔。例如："SharpnessDecorator;SmiteDecorator"。
/// 根据提供的字符串，为武器添加相应的附魔装饰器。
/// </summary>
/// <param name="decoratorString">附魔类型的字符串，各类型通过分号分隔。</param>
/// <exception cref="NotImplementedException">如果方法未实现，则抛出此异常。</exception>
public void MakeEnchantedWeapon(string decoratorString)
{
    bool hasValidDecorator = TryParseDecoratorTypes(decoratorString, out var decorators);
    if (!hasValidDecorator) throw new ArgumentException("提供的附魔类型字符串无效或包含无法识别的类型名称。", decoratorString);

    // 根据装饰器类型实现武器附魔的具体逻辑 ↓

    throw new NotImplementedException("MakeEnchantedWeapon 方法的具体实现尚未完成。需要根据解析出的装饰器类型为武器应用相应的附魔。");

    _weapon.Use();
}
```

对于 `IsValidDecoratorType`，你可能不知道我们为什么需要实现它。
那是因为在另一个函数 `TryParseDecoratorTypes` 中，我将解析所有存在的类型和装饰器类型。
然而，我不知道你如何实现你的装饰器类型，所以我留了一个接口来提供灵活性。

你可以查看 `IsValidDecoratorType` 是如何实现的，以获得更好的理解。

```csharp
// 遍历所有提取的类型名
foreach (string str in decoratorStrings)
{
    string myDecoratorString = str.Trim(); // 清除类型名周围的空白字符
    var type = Type.GetType(myDecoratorString, false); // 尝试获取对应的 Type 对象，不抛出异常

    // 验证获取的 Type 是否有效且为合法的装饰器类型
    if (type != null && IsValidDecoratorType(type))
    {
        decorators.Add(type);
    }
    else
    {
        // 如果无法找到类型或类型不是有效的装饰器，记录中文错误信息
        Debug.LogError($"未找到装饰器类型或类型不合法: {myDecoratorString}");
        return false;
    }
}
```

## 第三步：将 Weapon 类挂在在 Item 对象上

由于我仅知道我实现的 `IWeapon` 接口，并且可以在名为 `Item` 的对象中寻找到实现了这一接口的组件，因此请确保将你的 `Weapon` 类挂载到 `Item` 对象上，以保证代码能够正常运行。我已经在编辑器中高亮显示了 `Item` 对象。如果 `Weapon` 类没有正确挂载，你将看到以下错误信息：

> 未在 `Item` 或其子对象中找到实现了 `IWeapon` 接口的组件。请确保你的武器类已经挂载在该对象上。

## 第四步：测试时间！

最后，你可以启动游戏并点击按钮。
如果你的实现正确，你将正确地调用 `Use()` 函数。

这里是演示视频:

如果你遇到困难，可以随时：

1.  查看我在 [main 分支](https://github.com/RUI-0517/decorator-pattern) 上的代码。
2.  通过哔哩哔哩私信联系我：[-ANVER-](https://space.bilibili.com/29389283)
3.  给我发电子邮件：question@boopoo.co

再见:)
