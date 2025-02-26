using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;

    private Vector3 _cameraPosition;

    private void Awake()
    {
        _cameraPosition = transform.position;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null) return;

        _cameraPosition.x = Player.transform.position.x;
        _cameraPosition.y = Player.transform.position.y;

        transform.position = _cameraPosition;
    }
}
