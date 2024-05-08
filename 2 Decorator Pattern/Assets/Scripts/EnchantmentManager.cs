using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnchantmentManager : MonoBehaviour
{
    /// <summary>s
    /// 要被附魔的武器
    /// </summary>
    [SerializeField] private GameObject _item;

    private IWeapon _weapon;


    [UsedImplicitly]
    private void Start()
    {
        _weapon = _item.GetComponentInChildren<IWeapon>();

        if (_weapon != null) return;
        Debug.LogError($"未在 {_item.name} 或其子对象中找到实现了 IWeapon 接口的组件。请确保你的武器类已经挂载在该对象上。");

#if UNITY_EDITOR
        UnityEditor.EditorGUIUtility.PingObject(_item);
#endif
    }


    /// <summary>
    /// 对武器进行附魔。此方法将接受一个包含一个或多个附魔类型的字符串输入。
    /// 输入字符串中的附魔类型应以分号 (';') 分隔。例如："SharpnessDecorator;SmiteDecorator"。
    /// 根据提供的字符串，为武器添加相应的附魔装饰器。
    /// </summary>
    /// <param name="decoratorString">附魔类型的字符串，各类型通过分号分隔。</param>
    /// <exception cref="NotImplementedException">如果方法未实现，则抛出此异常。</exception>
    [UsedImplicitly]
    public void MakeEnchantedWeapon(string decoratorString)
    {
        bool hasValidDecorator = TryParseDecoratorTypes(decoratorString, out var decorators);
        if (!hasValidDecorator) throw new ArgumentException("提供的附魔类型字符串无效或包含无法识别的类型名称。", decoratorString);

        // 根据装饰器类型实现武器附魔的具体逻辑 ↓

        IWeapon currentWeapon = _weapon;

        foreach (Type decorator in decorators)
        {
            bool hasValidConstructor = decorator.GetConstructor(new[] { typeof(IWeapon) }) != null;
            if (!hasValidConstructor) continue;
            currentWeapon = (IWeapon)Activator.CreateInstance(decorator, currentWeapon);
        }

        _weapon = currentWeapon;
        _weapon.Use();
    }

    /// <summary>
    /// 验证给定的类型是否为有效的装饰器。
    /// </summary>
    /// <param name="type">需要验证的类型。</param>
    /// <returns>如果类型是一个有效的装饰器，则返回 true；否则返回 false。</returns>
    private static bool IsValidDecoratorType(Type type)
    {
        return type.IsSubclassOf(typeof(WeaponDecorator));
    }

    /// <summary>
    /// 尝试解析装饰器类型。
    /// </summary>
    /// <param name="decoratorString">包含一个或多个用分号分隔的类型名的字符串。</param>
    /// <param name="decorators">输出参数，装饰器类型数组。如果解析成功，此数组将被填充，否则为 null。</param>
    /// <returns>如果所有类型名都成功解析为有效的 Type 对象，则返回 true；如果任何一个类型名无效，返回 false。</returns>
    public static bool TryParseDecoratorTypes(string decoratorString, out List<Type> decorators)
    {
        // 检查输入字符串是否为空或仅包含空格
        if (string.IsNullOrEmpty(decoratorString))
        {
            decorators = null;
            return false;
        }

        // 按分号分隔字符串，获取可能的类型名数组
        string[] decoratorStrings = decoratorString.Split(';');
        if (decoratorStrings.Length == 0)
        {
            decorators = null;
            return false;
        }

        // 初始化类型数组
        decorators = new List<Type>(decoratorStrings.Length);

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

        return true;
    }
}