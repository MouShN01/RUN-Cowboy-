using UnityEngine;

public class Regdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    private void Awake()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();

        DisableRagdoll(); // Вимикаємо Ragdoll на старті
    }

    public void EnableRagdoll()
    {
        animator.enabled = false;

        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = false;
        }

        foreach (var col in _colliders)
        {
            col.enabled = true;
        }
    }

    public void DisableRagdoll()
    {
        animator.enabled = true;

        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (var col in _colliders)
        {
            col.enabled = false;
        }
    }
}