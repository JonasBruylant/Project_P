using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private Dictionary<int, bool> _itemData = new();
    

    void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    public bool HasItemFromID(int id)
    {
        if (id < 0) return false;

        if (!_itemData.ContainsKey(id)) return false;

        return _itemData[id];
    }

    public void CollectItem(int id)
    {
        if (id < 0) return;

        if (!_itemData.TryAdd(id, true))
        {
            Debug.LogError("<color=red> Item has already been collected. Check item ID's </color>");
            return;
        }

         _itemData[id] = true;
    }
}
