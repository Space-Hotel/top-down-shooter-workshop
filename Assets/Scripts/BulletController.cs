using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeDuration = 3f;
    public float damage = 5f;

    private float lifetime = 0f;
    private void Update()
    {
        if (lifetime >= lifeDuration)
        {
            Destroy(gameObject);
        }
        lifetime += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
