using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager
{
    // Key : ���ڿ�, Value : ObjectPool
    private static Dictionary<string, IPool> poolDic 
        = new Dictionary<string, IPool>();

    /// <summary>
    /// Pool �����Ҷ� �� �Լ� ���
    /// </summary>
    /// <typeparam name="T">Pool �� ������Ʈ�� �������� ������Ʈ</typeparam>
    /// <param name="prefab">����ų ������Ʈ</param>
    /// <param name="parent">����ų ������Ʈ�� �θ��� Transform</param>
    /// <param name="count">����ų ������Ʈ�� ��</param>
    public static void CreatePool<T>(GameObject prefab, Transform parent, int count = 5) where T : MonoBehaviour
    {
        ObjectPool<T> pool = new ObjectPool<T>(prefab, parent, count);
        poolDic.Add(prefab.name, pool);
    }

    /// <summary>
    /// Pool ���� ������Ʈ ������ �� ���
    /// </summary>
    /// <typeparam name="T">CreatePool �Ҷ� ����ߴ� ������Ʈ �ۼ�</typeparam>
    /// <param name="prefab">������ ������Ʈ�� prefab</param>
    /// <returns>������Ʈ�� Ȱ��ȭ ��Ű�� ��������</returns>
    public static T GetItem<T>(GameObject prefab) where T : MonoBehaviour
    {
        ObjectPool<T> pool = (ObjectPool<T>)poolDic[prefab.name];
        return pool.GetOrCreate();
    }
}