using UnityEngine;

public class Agent : MonoBehaviour
{

    public float speed;
    public float obstacleDistance;
    public float marginDistance;

    private Rigidbody rb;

    void Start()
    {
        //speed = 5.0f;
        //obstacleDistance = 2f;
        //marginDistance = 3f;
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, obstacleDistance))
        {
            Vector3 newDirection = Vector3.Reflect(transform.forward, hit.normal);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if (!Physics.Raycast(transform.position + transform.forward * marginDistance + Vector3.up, Vector3.down, out _, Vector3.up.magnitude))
        {
            transform.rotation = Quaternion.LookRotation(-transform.forward);
        }

        Vector3 newPosition = transform.position + speed * Time.deltaTime * transform.forward;
        rb.MovePosition(newPosition);

    }
}