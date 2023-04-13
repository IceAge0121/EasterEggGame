using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupDrop : MonoBehaviour
{

    [SerializeField] private Transform _playerCameraTransform;
    [SerializeField] private Transform _objectGrabPointTransform;
    [SerializeField] private LayerMask _pickupLayerMask;

    private ObjectGrabble objectGrabbable;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {     
            if (objectGrabbable == null)
            {     
                //If player is not carrying an object, try to grab
                float pickupDistance = 6f;
                if (Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance, _pickupLayerMask))
                {
                    //Debug.DrawRay(transform.position, transform.forward * pickupDistance, Color.yellow, 1, true);
                    //Debug.Log(raycastHit.transform);
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(_objectGrabPointTransform);
                        Debug.Log(objectGrabbable);
                    }
                }
            }
            else
            {
                //If player is carrying osmething, drop it
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }
}
