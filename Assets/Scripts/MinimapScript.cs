using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform target;
    public float distance = 10;

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y + distance, target.position.z);
    }
}