using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSunDirection : MonoBehaviour
{
    private float speed = 15f;

    void Update()
    {
        this.transform.Rotate(Vector3.right, speed * Time.deltaTime);
    }
}
