using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] CharacterPools;

    static Dictionary<GameObject, Pool> dictionary;

    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();

        Initialize(CharacterPools);
    }

#if UNITY_EDITOR
    //�s�边����ե�
    void OnDestroy()
    {
        CheckPoolSize(CharacterPools);
    }

    void CheckPoolSize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
            if(pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                                string.Format("��H��:{0} ��{1}�Ӧb�B��A�j�L�򥻼�{2}��",
                                pool.Prefab.name,
                                pool.RuntimeSize,
                                pool.Size));
            }
        }
    }
#endif

    //��l�ƥ�����H��
    void Initialize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
            #if UNITY_EDITOR
                //�Y�r�夤�w�s�J����N���L
                if (dictionary.ContainsKey(pool.Prefab))
                {
                    Debug.LogError("Same prefab in multiple pools: Prefab: " + pool.Prefab.name);

                    continue;
                }
            #endif

                dictionary.Add(pool.Prefab, pool);

                Transform poolParent = new GameObject("Pool :  " + pool.Prefab.name).transform;

                poolParent.parent = transform;
                pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// <para>�ھڶǤJ��<paramref name="prefab"></paramref>�Ѽ�,��^��H�����w�Ʀn���C����H</para>
    /// </summary>
    /// <paramref name="prefab">
    /// <para>���w���C����H�w�s��</para>
    /// </param>
    /// <returns>
    /// <para>��H�����w�Ʀn���C����H</para>
    /// </returns>
    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find Prefab: " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].preparedObject();
    }

    /// <summary>
    /// <para>�ھڶǤJ��prefab�Ѽ�,�bposition�ѼƦ�m�����H�����w�Ʀn���C����H</para>
    /// </summary>
    /// <paramref name="prefab">
    /// <para>���w���C����H�w�s��</para>
    /// </param>
    /// <returns>
    /// <para>��H�����w�Ʀn���C����H</para>
    /// </returns>
    public static GameObject Release(GameObject prefab,Vector3 position)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find Prefab: " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].preparedObject(position);
    }

    /// <summary>
    /// <para>�ھڶǤJ��prefab�Ѽ�,rotation�Ѽ�,�bposition�ѼƦ�m�����H�����w�Ʀn���C����H</para>
    /// </summary>
    /// <paramref name="prefab">
    /// <para>���w���C����H�w�s��</para>
    /// </param>
    /// <returns>
    /// <para>��H�����w�Ʀn���C����H</para>
    /// </returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find Prefab: " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].preparedObject(position,rotation);
    }

    /// <summary>
    /// <para>�ھڶǤJ��prefab�Ѽ�,rotation�Ѽ�,localScale�Ѽ�,�bposition�ѼƦ�m�����H�����w�Ʀn���C����H</para>
    /// </summary>
    /// <paramref name="prefab">
    /// <para>���w���C����H�w�s��</para>
    /// </param>
    /// <returns>
    /// <para>��H�����w�Ʀn���C����H</para>
    /// </returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find Prefab: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position, rotation, localScale);
        
    }
}
