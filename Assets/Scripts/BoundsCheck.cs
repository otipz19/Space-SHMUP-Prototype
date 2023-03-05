using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предотвращает выход игрового объекта за границы экрана.
/// Работает только с ортографической камерой в позиции [0, 0, 0]
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 4f;
    public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public float camWidth;
    public float camHeight;
    public bool isOnScreen = true;

    [HideInInspector]
    public bool offDown, offUp, offLeft, offRight;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = Camera.main.aspect * camHeight;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            offRight = true;
        }
        else
        {
            offRight = false;
        }
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            offLeft = true;
        }
        else
        {
            offLeft = false;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            offUp = true;
        }
        else
        {
            offUp = false;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            offDown = true;
        }
        else
        {
            offDown = false;
        }

        isOnScreen = !(offDown || offLeft || offRight || offUp);
        
        if(keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offUp = offRight = offLeft = offDown = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
            Gizmos.DrawWireCube(Vector3.zero, boundSize);
        }
    }
}
