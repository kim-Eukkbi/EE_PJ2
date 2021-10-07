using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager
{
    // Key : 문자열, Value : ObjectPool
    private static Dictionary<string, IPool> poolDic 
        = new Dictionary<string, IPool>();

    /// <summary>
    /// Pool 생성할때 이 함수 사용
    /// </summary>
    /// <typeparam name="T">Pool 할 오브젝트의 공통적인 컴포넌트</typeparam>
    /// <param name="prefab">양산시킬 오브젝트</param>
    /// <param name="parent">양산시킬 오브젝트의 부모의 Transform</param>
    /// <param name="count">양산시킬 오브젝트의 수</param>
    public static void CreatePool<T>(GameObject prefab, Transform parent, int count = 5) where T : MonoBehaviour
    {
        ObjectPool<T> pool = new ObjectPool<T>(prefab, parent, count);
        poolDic.Add(prefab.name, pool);
    }

    /// <summary>
    /// Pool 안의 오브젝트 가져올 때 사용
    /// </summary>
    /// <typeparam name="T">CreatePool 할때 사용했던 컴포넌트 작성</typeparam>
    /// <param name="prefab">가져올 오브젝트의 prefab</param>
    /// <returns>오브젝트를 활성화 시키고 리턴해줌</returns>
    public static T GetItem<T>(GameObject prefab) where T : MonoBehaviour
    {
        ObjectPool<T> pool = (ObjectPool<T>)poolDic[prefab.name];
        return pool.GetOrCreate();
    }
}
