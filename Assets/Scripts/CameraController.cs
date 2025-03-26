using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;
    
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;

        Vector3 playerPos = new Vector3(player.position.x, player.position.y, -10);
        transform.position = playerPos;
    }

    void Update()
    {
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, -10);
        transform.position = playerPos;
    }
}
