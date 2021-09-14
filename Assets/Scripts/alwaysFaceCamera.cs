using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alwaysFaceCamera : MonoBehaviour
{
    [SerializeField] private Transform mainCameraTransform;
    // Start is called before the first frame update
    void Start()
    {

        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
            mainCameraTransform.rotation * Vector3.up);
    }
}
