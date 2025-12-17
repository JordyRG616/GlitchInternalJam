using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform charOne;
    [SerializeField] private Transform charTwo;
    [Space]
    [SerializeField] private float smoothSpeed;
    [Space] 
    [SerializeField] private Vector2 distanceMinMax; 
    [SerializeField] private Vector2 zoomRange;
    
    private Camera _camera;
    private Vector2 currentVelocity;


    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        Center();
        Zoom();
    }

    private void Center()
    {
        Vector3 pos = Vector2.Lerp(charOne.position, charTwo.position, 0.5f);
        pos = Vector2.SmoothDamp(transform.position, pos, ref currentVelocity, smoothSpeed);
        pos.z = -10;
        transform.position = pos;
    }

    private void Zoom()
    {
        var distance = Vector2.Distance(charOne.position, charTwo.position);
        distance = Mathf.Clamp(distance, distanceMinMax.x, distanceMinMax.y);
        distance /= distanceMinMax.y - distanceMinMax.x;
        var zoom = Mathf.Lerp(zoomRange.x, zoomRange.y, distance);
        
        _camera.orthographicSize = zoom;
    }
}
