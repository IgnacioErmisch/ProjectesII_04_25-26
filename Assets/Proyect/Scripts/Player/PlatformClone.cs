using UnityEngine;

public class PlatformClone : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int direction = 0; 

    void Update()
    {
        if (direction != 0)
        {
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