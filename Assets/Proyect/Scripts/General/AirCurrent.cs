using System.Collections.Generic;
using UnityEngine;

public class AirCurrent : MonoBehaviour
{
    [Header("Corriente")]
    [SerializeField] private Vector2 currentDirection = Vector2.right;
    [SerializeField] private float pushForce = 15f;
    [SerializeField] private float entryImpulse = 20f;

    [Header("Bloqueo por clon grande")]
    [SerializeField] private LayerMask bigCloneLayer;
    [SerializeField] private float blockCheckDistance = 6f;

    [Header("Objetos afectados")]
    [SerializeField] private LayerMask affectedLayers;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;

    private readonly List<Rigidbody2D> objectsInside = new List<Rigidbody2D>();
    private BoxCollider2D triggerArea;
    private Collider2D bigCloneBlocker = null;

    private void Awake()
    {
        triggerArea = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        bigCloneBlocker = GetBlockingClone();
        Vector2 dir = currentDirection.normalized;

        for (int i = objectsInside.Count - 1; i >= 0; i--)
        {
            if (objectsInside[i] == null)
            {
                objectsInside.RemoveAt(i);
                continue;
            }

            Rigidbody2D rb = objectsInside[i];

            if (IsProtectedByClone(rb, dir))
                continue;

            PlayerMovement pm = rb.GetComponent<PlayerMovement>();

            if (pm != null)
            {
                Vector2 currentVel = rb.linearVelocity;
                float componentAlongCurrent = Vector2.Dot(currentVel, dir);

                if (componentAlongCurrent < 0f)
                    currentVel -= componentAlongCurrent * dir;

                float newComponent = Vector2.Dot(currentVel, dir);
                if (newComponent < pushForce)
                    currentVel += dir * (pushForce - newComponent);

                pm.SetExternalVelocity(currentVel);
            }
            else
            {
                rb.AddForce(dir * pushForce, ForceMode2D.Force);
            }
        }
    }

    private Collider2D GetBlockingClone()
    {
        if (triggerArea == null) return null;

        Collider2D[] hits = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(bigCloneLayer);
        filter.useTriggers = false;

        int count = Physics2D.OverlapCollider(triggerArea, filter, hits);

        for (int i = 0; i < count; i++)
        {
            if (hits[i] != null)
                return hits[i];
        }

        return null;
    }

    private bool IsProtectedByClone(Rigidbody2D rb, Vector2 dir)
    {
        if (bigCloneBlocker == null) return false;

        Vector2 dirAbs = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

        float cloneProjection = Vector2.Dot((Vector2)bigCloneBlocker.bounds.center, dir);
        float cloneExtent = Vector2.Dot((Vector2)bigCloneBlocker.bounds.extents, dirAbs);
        float cloneLeadingEdge = cloneProjection + cloneExtent;

        float objectProjection = Vector2.Dot(rb.position, dir);

        return objectProjection > cloneLeadingEdge;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsAffectedLayer(other.gameObject)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        if (!objectsInside.Contains(rb))
            objectsInside.Add(rb);

        Vector2 dir = currentDirection.normalized;
        if (IsProtectedByClone(rb, dir)) return;

        PlayerMovement pm = rb.GetComponent<PlayerMovement>();

        if (pm != null)
            pm.SetExternalVelocity(dir * entryImpulse);
        else
            rb.AddForce(dir * entryImpulse, ForceMode2D.Impulse);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsAffectedLayer(other.gameObject)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null) objectsInside.Remove(rb);
    }

    private bool IsAffectedLayer(GameObject go)
    {
        return ((1 << go.layer) & affectedLayers) != 0;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box == null) return;

        Vector3 center = transform.position + (Vector3)(box.offset);
        Vector3 dir = (Vector3)currentDirection.normalized;
        bool blocked = bigCloneBlocker != null;

        Gizmos.color = blocked ? new Color(0f, 1f, 0f, 0.25f) : new Color(0f, 0.6f, 1f, 0.25f);
        Gizmos.DrawCube(center, box.size);
        Gizmos.color = blocked ? Color.green : Color.cyan;
        Gizmos.DrawWireCube(center, box.size);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(center - dir * 0.5f, center + dir * 1f);
        Vector3 right = Vector3.Cross(dir, Vector3.forward).normalized * 0.2f;
        Gizmos.DrawLine(center + dir * 1f, center + dir * 0.6f + right);
        Gizmos.DrawLine(center + dir * 1f, center + dir * 0.6f - right);

        if (blocked && bigCloneBlocker != null)
        {
            Vector2 dir2 = currentDirection.normalized;
            Vector2 dirAbs = new Vector2(Mathf.Abs(dir2.x), Mathf.Abs(dir2.y));
            float cloneProj = Vector2.Dot((Vector2)bigCloneBlocker.bounds.center, dir2);
            float cloneExtent = Vector2.Dot((Vector2)bigCloneBlocker.bounds.extents, dirAbs);
            float leadingEdge = cloneProj + cloneExtent;

            Vector3 shieldPos = center + dir * (leadingEdge - Vector2.Dot((Vector2)center, dir2));
            Gizmos.color = Color.yellow;
            Vector3 perp = Vector3.Cross(dir, Vector3.forward).normalized;
            Gizmos.DrawLine(shieldPos - perp * 2f, shieldPos + perp * 2f);
        }
    }
}