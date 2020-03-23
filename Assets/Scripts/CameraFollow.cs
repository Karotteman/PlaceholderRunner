using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, 0.06f);

        offset = transform.position - target.position;
        transform.rotation = Quaternion.LookRotation(-offset);
    }
}
