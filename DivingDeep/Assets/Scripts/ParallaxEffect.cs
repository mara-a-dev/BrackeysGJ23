using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;

    private Transform cam;
    private Vector3 lastCamPos;

    private void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }
    private void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCamPos;
        transform.position += deltaMovement * parallaxMultiplier;
        lastCamPos = cam.position;
    }
}
