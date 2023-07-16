using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerAsset;

    private float cameraDist;
    void Start()
    {
        cameraDist = transform.position.z;
    }

    void Update()
    {
        Vector3 newPos = playerAsset.transform.position;
        newPos.z = cameraDist;
        transform.position = newPos;
    }
}
