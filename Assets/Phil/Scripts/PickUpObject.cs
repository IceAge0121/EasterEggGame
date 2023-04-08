using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;


public class PickUpObject : MonoBehaviour
{
    [SerializeField] Transform playerCameraTransform;

    [SerializeField] private LayerMask pickUpLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            float _pickUpDistance = 2f;
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, _pickUpDistance, pickUpLayerMask))
            {
                Debug.Log(raycastHit.transform);
                if (raycastHit.transform.TryGetComponent(out ObjectGrabble objectGrabble))
                {
                    Debug.Log(objectGrabble);
                }
            }   
        }
    }
}
