using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Pool
{
    public GameObject Prefab { get => prefab; }

    //������ƶq
    public int Size { get => size; }
    //������B��ɪ��ƶq
    public int RuntimeSize { get => queue.Count; }
    [Header("�n���ͪ���l��")]
    [SerializeField] GameObject prefab;
    [Header("������̤j�ƶq")]
    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    Transform parent;

    //��l�ƹ�H��
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy()); //�[�J����
        }
    }

    //�s�y����
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }

    //���X
    GameObject AvailableObject()
    {
        GameObject availableObject = null;
        //Peek() ��^�Ĥ@��   �åB�L���}�Үɤ~�ե�
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject =  queue.Dequeue(); //���o�}�Y�Ĥ@��
        }
        else
        {
            availableObject = Copy();
        }

        queue.Enqueue(availableObject);  //��^����

        return availableObject;
    }

    //���X����ó]�m
    public GameObject preparedObject()
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position,Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }
}
