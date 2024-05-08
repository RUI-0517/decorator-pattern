using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnchantmentManager : MonoBehaviour
{
    /// <summary>s
    /// Ҫ����ħ������
    /// </summary>
    [SerializeField] private GameObject _item;

    private IWeapon _weapon;


    [UsedImplicitly]
    private void Start()
    {
        _weapon = _item.GetComponentInChildren<IWeapon>();

        if (_weapon != null) return;
        Debug.LogError($"δ�� {_item.name} �����Ӷ������ҵ�ʵ���� IWeapon �ӿڵ��������ȷ������������Ѿ������ڸö����ϡ�");

#if UNITY_EDITOR
        UnityEditor.EditorGUIUtility.PingObject(_item);
#endif
    }


    /// <summary>
    /// ���������и�ħ���˷���������һ������һ��������ħ���͵��ַ������롣
    /// �����ַ����еĸ�ħ����Ӧ�Էֺ� (';') �ָ������磺"SharpnessDecorator;SmiteDecorator"��
    /// �����ṩ���ַ�����Ϊ���������Ӧ�ĸ�ħװ������
    /// </summary>
    /// <param name="decoratorString">��ħ���͵��ַ�����������ͨ���ֺŷָ���</param>
    /// <exception cref="NotImplementedException">�������δʵ�֣����׳����쳣��</exception>
    [UsedImplicitly]
    public void MakeEnchantedWeapon(string decoratorString)
    {
        bool hasValidDecorator = TryParseDecoratorTypes(decoratorString, out var decorators);
        if (!hasValidDecorator) throw new ArgumentException("�ṩ�ĸ�ħ�����ַ�����Ч������޷�ʶ����������ơ�", decoratorString);

        // ����װ��������ʵ��������ħ�ľ����߼� ��

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
    /// ��֤�����������Ƿ�Ϊ��Ч��װ������
    /// </summary>
    /// <param name="type">��Ҫ��֤�����͡�</param>
    /// <returns>���������һ����Ч��װ�������򷵻� true�����򷵻� false��</returns>
    private static bool IsValidDecoratorType(Type type)
    {
        return type.IsSubclassOf(typeof(WeaponDecorator));
    }

    /// <summary>
    /// ���Խ���װ�������͡�
    /// </summary>
    /// <param name="decoratorString">����һ�������÷ֺŷָ������������ַ�����</param>
    /// <param name="decorators">���������װ�����������顣��������ɹ��������齫����䣬����Ϊ null��</param>
    /// <returns>����������������ɹ�����Ϊ��Ч�� Type �����򷵻� true������κ�һ����������Ч������ false��</returns>
    public static bool TryParseDecoratorTypes(string decoratorString, out List<Type> decorators)
    {
        // ��������ַ����Ƿ�Ϊ�ջ�������ո�
        if (string.IsNullOrEmpty(decoratorString))
        {
            decorators = null;
            return false;
        }

        // ���ֺŷָ��ַ�������ȡ���ܵ�����������
        string[] decoratorStrings = decoratorString.Split(';');
        if (decoratorStrings.Length == 0)
        {
            decorators = null;
            return false;
        }

        // ��ʼ����������
        decorators = new List<Type>(decoratorStrings.Length);

        // ����������ȡ��������
        foreach (string str in decoratorStrings)
        {
            string myDecoratorString = str.Trim(); // �����������Χ�Ŀհ��ַ�
            var type = Type.GetType(myDecoratorString, false); // ���Ի�ȡ��Ӧ�� Type ���󣬲��׳��쳣

            // ��֤��ȡ�� Type �Ƿ���Ч��Ϊ�Ϸ���װ��������
            if (type != null && IsValidDecoratorType(type))
            {
                decorators.Add(type);
            }
            else
            {
                // ����޷��ҵ����ͻ����Ͳ�����Ч��װ��������¼���Ĵ�����Ϣ
                Debug.LogError($"δ�ҵ�װ�������ͻ����Ͳ��Ϸ�: {myDecoratorString}");
                return false;
            }
        }

        return true;
    }
}