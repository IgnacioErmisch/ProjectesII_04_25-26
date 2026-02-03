using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class CatapultPlatform : MonoBehaviour
{
    [Header("Activation Methods")]
    [SerializeField] private bool useImpactForce = true;
    [SerializeField] private bool useDashDetection = true;
    [SerializeField] private float requiredImpactForce = 2f;
    [SerializeField] private float resetDelay = 0.5f;

    [Header("Visual Feedback")]
    [SerializeField] private float pressedOffset = 0.3f;
    [SerializeField] private float pressAnimationSpeed = 10f;
    [SerializeField] private ParticleSystem impactEffect;
    [SerializeField] private AudioClip impactSound;

    [Header("References")]
    [SerializeField] private CatapultSystem catapultSystem;

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isPressed = false;
    private AudioSource audioSource;
    private float lastImpactForce = 0f; // NUEVO: Almacenar la fuerza del último impacto

    private void Awake()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && impactSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Auto-encontrar el CatapultSystem si no esta asignado
        if (catapultSystem == null)
        {
            catapultSystem = FindFirstObjectByType<CatapultSystem>();

            if (showDebug)
            {
                if (catapultSystem != null)
                    Debug.Log($"[Catapult] CatapultSystem encontrado automaticamente: {catapultSystem.gameObject.name}");
                else
                    Debug.LogError("[Catapult] No se encontro CatapultSystem en la escena!");
            }
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * pressAnimationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BigClone"))
        {
            if (isPressed)
            {
                if (showDebug)
                    Debug.Log("[Catapult] Plataforma ya esta presionada, ignorando...");
                return;
            }

            bool shouldActivate = false;
            float impactForce = 0f;

            if (useDashDetection)
            {
                BigCloneAttack bigCloneAttack = collision.gameObject.GetComponent<BigCloneAttack>();

                if (bigCloneAttack != null && bigCloneAttack.isDashing)
                {
                    if (showDebug)
                        Debug.Log("[Catapult] BigClone esta haciendo dash! Activando catapulta.");
                    shouldActivate = true;
                    // En caso de dash, usar una fuerza mayor
                    impactForce = collision.relativeVelocity.magnitude * 1.5f;
                }
            }

            if (useImpactForce && !shouldActivate)
            {
                // MODIFICADO: Usar la magnitud completa de la velocidad relativa
                impactForce = collision.relativeVelocity.magnitude;

                if (showDebug)
                    Debug.Log($"[Catapult] Impacto con velocidad relativa: {impactForce} (Requerido: {requiredImpactForce})");

                if (impactForce >= requiredImpactForce)
                {
                    shouldActivate = true;
                }
            }

            if (shouldActivate)
            {
                // MODIFICADO: Pasar la fuerza de impacto al método
                ActivateCatapult(impactForce);
            }
            else if (showDebug)
            {
                Debug.Log("[Catapult] No se cumplen las condiciones para activar la catapulta");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BigClone") && !isPressed)
        {
            if (useDashDetection)
            {
                BigCloneAttack bigCloneAttack = collision.gameObject.GetComponent<BigCloneAttack>();

                if (bigCloneAttack != null && bigCloneAttack.isDashing)
                {
                    if (showDebug)
                        Debug.Log("[Catapult] BigClone empezo a hacer dash sobre la plataforma!");

                    float impactForce = collision.relativeVelocity.magnitude * 1.5f;
                    ActivateCatapult(impactForce);
                }
            }
        }
    }

    // MODIFICADO: Ahora acepta la fuerza de impacto como parámetro
    private void ActivateCatapult(float impactForce)
    {
        if (showDebug)
            Debug.Log($"[Catapult] Catapulta activada con fuerza de impacto: {impactForce}");

        isPressed = true;
        lastImpactForce = impactForce; // Guardar para pasarla al sistema
        targetPosition = originalPosition - Vector3.up * pressedOffset;
        PlayEffects();

        if (catapultSystem != null)
        {
            if (showDebug)
                Debug.Log("[Catapult] Llamando a OnCatapultActivated() del sistema");

            // MODIFICADO: Pasar la fuerza de impacto al sistema
            catapultSystem.OnCatapultActivated(lastImpactForce);
        }
        else
        {
            Debug.LogError("[Catapult] CatapultSystem es null! No se puede activar el lanzamiento.");
        }

        Invoke(nameof(ResetPlatform), resetDelay);
    }

    private void ResetPlatform()
    {
        if (showDebug)
            Debug.Log("[Catapult] Reseteando plataforma catapulta");
        targetPosition = originalPosition;
        isPressed = false;
        lastImpactForce = 0f;
    }

    private void PlayEffects()
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        if (audioSource != null && impactSound != null)
        {
            audioSource.PlayOneShot(impactSound);
        }
    }

    public void ForceReset()
    {
        CancelInvoke(nameof(ResetPlatform));
        targetPosition = originalPosition;
        isPressed = false;
        lastImpactForce = 0f;
        transform.position = originalPosition;
    }

    // NUEVO: Método para obtener la última fuerza de impacto (por si se necesita)
    public float GetLastImpactForce()
    {
        return lastImpactForce;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = Application.isPlaying ? originalPosition : transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, transform.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos - Vector3.up * pressedOffset, transform.localScale);
    }
}