using UnityEngine;

public class Picker : MonoBehaviour
{
    [SerializeField] private Transform logHolder;
    [SerializeField] private Transform helperLog;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationSpeed; 
    [SerializeField] private float maxSlopePlace;
    [SerializeField] private float pickupRange;
    [SerializeField] private float placeRange; 

    private IPickable currentItem;
    private bool isPlayerPlacing;
    private float logRadius;

    private void Start()
    {
        InputManager.Instance.OnPlayerPrimaryAction += InputManager_OnPlayerPrimaryAction;
        InputManager.Instance.OnPlayerSecondaryActionPerformed += InputManager_OnPlayerSecondaryActionPerformed;
        InputManager.Instance.OnPlayerSecondaryActionCancelled += InputManager_OnPlayerSecondaryActionCancelled;

        logRadius = helperLog.GetComponent<CapsuleCollider>().radius;
    }

    private void InputManager_OnPlayerSecondaryActionCancelled()
    {
        isPlayerPlacing = false;
        helperLog.gameObject.SetActive(false);

        if (currentItem != null)
        {
            currentItem.Drop(helperLog.position, helperLog.rotation);
            currentItem = null;
        }
    }

    private void InputManager_OnPlayerSecondaryActionPerformed()
    {
        isPlayerPlacing = true;
    }

    private void Update()
    {
        if (currentItem == null) return;

        if (isPlayerPlacing)
        {
            helperLog.gameObject.SetActive(true);
            PlaceHelper();
        }
    }

    private void InputManager_OnPlayerPrimaryAction()
    {
        if (currentItem != null)
        {
            return;
        }

        currentItem = GetItem();

        if (currentItem != null)
        {
            currentItem.PickUp(logHolder);
        }
    }

    private IPickable GetItem()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.TryGetComponent(out IPickable pickable))
            {
                return pickable;
            }
        }
        return null;
    }

    private void PlaceHelper()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, placeRange))
        {
            if (Vector3.Distance(hit.normal, Vector3.up) < maxSlopePlace)
            {
                Vector3 position = hit.point;
                position.y += logRadius;
                helperLog.position = position;
            }
        }
    }
}