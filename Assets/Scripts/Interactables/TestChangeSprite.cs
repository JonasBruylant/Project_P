using UnityEngine;

public class TestChangeSprite : MonoBehaviour, IInteractable
{

    private SpriteRenderer _sr;
    public Sprite Sprite;

    public bool disablePlayerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite()
    {
        _sr.sprite = Sprite;
    }

    public void Interact()
    {
        ChangeSprite();

        //TO DO: Check if player movement has to be disabled, switch action maps from player to UI
    }
}
