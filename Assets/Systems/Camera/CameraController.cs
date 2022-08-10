using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementTime;

    [SerializeField] private Vector3 newPosition;

    [SerializeField] private Quaternion newRotation;
    [SerializeField] private float rotationAmount;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector3 zoomAmount;
    [SerializeField] private Vector3 newZoom;

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = transform.localPosition;
        newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        handlerMovement();
    }

    void handlerMovement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition += (transform.forward * (-movementSpeed));
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if(Input.GetKey(KeyCode.A)){
            newPosition += (transform.right * (-movementSpeed));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if(Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * (-rotationAmount));
        }
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

    }
}
