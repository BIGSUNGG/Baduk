using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    static public T GetOrAddComponent<T> (this GameObject self) where T : MonoBehaviour
    {
        var component = self.GetComponent<T>();

        if (component == null)
            component = self.AddComponent<T>();

        return component;
    }
}
