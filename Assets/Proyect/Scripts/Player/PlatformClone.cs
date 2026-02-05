using UnityEngine;

public class PlatformClone : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxDistance = 5f; 

    private int direction = 0;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (direction != 0)
        {
            float distanceFromStart = transform.position.y - startPosition.y;

            if (direction == 1 && distanceFromStart >= maxDistance)
            {
                Stop();
                return;
            }
            else if (direction == -1 && distanceFromStart <= -maxDistance)
            {
               
                Stop();
                return;
            }

            transform.Translate(Vector3.up * direction * moveSpeed * Time.deltaTime);
        }
    }

    public void MoveUp()
    {
        direction = 1;
    }

    public void MoveDown()
    {
        direction = -1;
    }

    public void Stop()
    {
        direction = 0;
    }
}