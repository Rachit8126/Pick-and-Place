using UnityEngine;

public interface IPickable
{
    public void PickUp(Transform parent);

    public void Drop(Vector3 position, Quaternion rotation);
}
