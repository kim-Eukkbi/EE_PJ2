using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IPool where T : MonoBehaviour
{
    //private Queue<T> m_queue; // 때에 따라서 Queue 자료구조를 사용하기도 함 
    private List<T> m_list = new List<T>();     // 하지만 대부분의 상황에서 List를 사용하는 것이 편하기에 List 사용
    private GameObject prefab;
    private Transform parent;

    public ObjectPool(GameObject prefab, Transform parent, int count = 5) // 생성자
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < count; i++) // count만큼 object를 만들고 비활성화 시킨 뒤 m_list에 넣어준다
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            T t = obj.GetComponent<T>();
            obj.SetActive(false);
            m_list.Add(t);
        }
    }

    public T GetOrCreate()
    {
        T t = m_list.Find(i => !i.gameObject.activeSelf); // 활성화 되지 않은 오브젝트를 find

        if (t == null) // 만약 find 해서 찾은게 없을 경우 새로 만든다
        {
            GameObject temp = GameObject.Instantiate(prefab, parent);
            temp.gameObject.SetActive(true);
            t = temp.GetComponent<T>();
            m_list.Add(t);
        }
        else
        {
            t.gameObject.SetActive(true);
        }

        return t;
    }

    public T Create()
    {
        T t = null;

        GameObject temp = GameObject.Instantiate(prefab, parent);
        temp.gameObject.SetActive(true);
        t = temp.GetComponent<T>();
        m_list.Add(t);

        return t;
    }
}
