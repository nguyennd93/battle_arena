using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    T _sample;
    List<T> _poolElements = new List<T>();

    public ObjectPool(T element)
    {
        _sample = element;
        element.gameObject.SetActive(false);
    }

    public T Get()
    {
        if (_poolElements.Count == 0)
        {
            var newElement = Object.Instantiate(_sample, _sample.transform.parent);
            newElement.gameObject.SetActive(true);
            return newElement;
        }

        var element = _poolElements[0];
        element.gameObject.SetActive(true);
        _poolElements.RemoveAt(0);
        return element;
    }

    public void Store(T element)
    {
        element.gameObject.SetActive(false);
        _poolElements.Add(element);
    }
}
