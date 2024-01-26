using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    //[Tooltip("Su dung ")]
    //public bool usedPrefabOnPooling;
    public T prefabObject;
    public List<T> pooling = new List<T>();
    protected virtual void Awake()
    {
        prefabObject.gameObject.SetActive(false);
        //ReturnAllObjectToPooling();
    }
    public T CreateObjectInPooling(T objectPrefab)
    {
        var objectNew = Instantiate(objectPrefab);
        objectNew.transform.localScale = objectPrefab.transform.lossyScale;
        objectNew.transform.position = objectPrefab.transform.position;
        objectNew.transform.SetParent(transform);
        objectNew.gameObject.SetActive(false);
        pooling.Add(objectNew);
        return objectNew;
    }
    public List<T> CreateObjectInPooling(T objectPrefab, int countInit)
    {
        List<T> listNew = new List<T>();
        for (int i = 0; i < countInit; i++)
        {
            var objectNew = CreateObjectInPooling(objectPrefab);
            listNew.Add(objectNew);
        }

        return listNew;
    }
    public T GetObjectInPooling()
    {
        T objectGet = null;
        for (int i = 0; i < pooling.Count; i++)
        {
            if (!pooling[i].gameObject.activeSelf)
            {
                objectGet = pooling[i];
                break;
            }
        }
        if (!objectGet)
        {
            if (prefabObject)
            {
                T prefab = prefabObject;
                objectGet = CreateObjectInPooling(prefab);
                //Debug.Log("Hết item và đã tự add thêm item");

            }
            else
            {
                //Debug.Log("Không thể tự add thêm vì không có mẫu");

            }
        }
        objectGet.gameObject.SetActive(true);
        return objectGet;
    }
    public T GetObjectInPooling(float durationReturn)
    {
        T objectGet = GetObjectInPooling();
        ReturnObjectToPooling(objectGet, durationReturn);
        return objectGet;
    }
    public void ReturnObjectToPooling(T objectUsed)
    {
        //DOTween.Kill(objectUsed.gameObject.GetInstanceID() + "DelayReturnPooling");
        objectUsed.transform.SetParent(transform);
        objectUsed.gameObject.SetActive(false);
    }
    public void ReturnObjectToPooling(T objectUsed, float delay)
    {
        //DOTween.Kill(objectUsed.gameObject.GetInstanceID() + "DelayReturnPooling");
        StartCoroutine(HandleAfterDisable(objectUsed));
        DOTween.Sequence()
            .SetUpdate(true)
            .AppendInterval(delay)
            .AppendCallback(() =>
            {
                ReturnObjectToPooling(objectUsed);
            })
            .SetId(objectUsed.gameObject.GetInstanceID() + "DelayReturnPooling")
            .Play();
    }
    public void ReturnAllObjectToPooling()
    {
        for (int i = 0; i < pooling.Count; i++)
        {
            ReturnObjectToPooling(pooling[i]);
        }
    }
    IEnumerator HandleAfterDisable(T objectUsed)
    {
        yield return new WaitUntil(() => objectUsed.gameObject.activeSelf == false);
        DOTween.Kill(objectUsed.gameObject.GetInstanceID() + "DelayReturnPooling");
    }
    public T GetPrefabInfor()
    {
        T objectGet = null;
        if (prefabObject)
        {
            objectGet = prefabObject;
            //Debug.Log("Hết item và đã tự add thêm item");

        }
        else
        {
            //Debug.Log("Không thể tự add thêm vì không có mẫu");

        }
        return objectGet;
    }
    //private void OnDestroy()
    //{
    //    DOTween.Kill(GetInstanceID() + "DelayReturnPooling");
    //}
}
