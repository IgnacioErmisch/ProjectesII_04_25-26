using UnityEngine;

public class BreakableZone : MonoBehaviour
{
    
    [SerializeField] private AntiCloneZone antiCloneZone; 
    [SerializeField] private PlayerCombatController player; 
    [SerializeField] private float breakRadius = 0.2f;     
    [SerializeField] private bool visualizeRange = true;
    [SerializeField] private Color gizmoColor = Color.cyan;

    private bool isBroken = false;

    private void Update()
    {
        if (isBroken || player == null || antiCloneZone == null) return;

        Transform pointer = player.GetAttackPoint();
        Vector3 pointerPosition = pointer != null ? pointer.position : player.transform.position;

        float distance = Vector3.Distance(transform.position, pointerPosition);

        if (distance <= breakRadius)
        {
            Break();
        }
    }

    private void Break()
    {
        isBroken = true;

        
        if (antiCloneZone != null)
        {
            antiCloneZone.gameObject.SetActive(false);
        }

        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (!visualizeRange) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, breakRadius);
    }
}
