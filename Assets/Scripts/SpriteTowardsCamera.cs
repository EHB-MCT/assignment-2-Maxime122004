using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTowardsCamera : MonoBehaviour
{
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    /**
     * Gets camera position and rotates the sprite to look at the camera.
     * Input: None
     * Action: Gets camera position and rotates sprite.
     * Output: None
     */
    private void LateUpdate()
    {
        Vector3 cameraPosition = _mainCamera.transform.position;
        cameraPosition.y = transform.position.y;
        transform.LookAt(cameraPosition);
        transform.Rotate(0f, 180f, 0f);
    }
}
