using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    public int ItemID;

    public void Interact()
    { 
        DataManager.Instance.CollectItem(ItemID);
        Debug.Log($"Item with ID {ItemID} collected");

        Destroy(gameObject);
    }

}
