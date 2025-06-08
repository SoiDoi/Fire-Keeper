using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform player;  
    public Vector3 offset;          
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            if (transform.position.x > 35)
            {
                transform.position = new Vector3(35,transform.position.y,transform.position.z);
            }
            if (transform.position.x < -35)
            {
                transform.position = new Vector3(-35, transform.position.y, transform.position.z);
            }
            if (transform.position.y > 42.5f)
            {
                transform.position = new Vector3(transform.position.x, 42.5f , transform.position.z);
            }
            if (transform.position.y < -41.5f)
            {
                transform.position = new Vector3(transform.position.x, -41.5f, transform.position.z);
            }
        }
    }
}
