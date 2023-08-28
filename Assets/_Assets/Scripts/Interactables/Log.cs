using UnityEngine;

public class Log : MonoBehaviour, IPickable
{
    private Transform originalParent;
    private Collider selfCollider;

    private void Awake()
    {
        selfCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        originalParent = transform.parent;
    }

    public void Drop(Vector3 position, Quaternion rotation)
    {
        selfCollider.enabled = true;
        transform.parent = originalParent;
        transform.SetLocalPositionAndRotation(position, rotation);
    }

    public void PickUp(Transform parent)
    {
        selfCollider.enabled = false;
        transform.parent = parent;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
