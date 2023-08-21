using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeThrowingController : MonoBehaviour
{
    public GameObject grenadeAsset;
    public Transform grenadeParentTransform;
    public Image grenadeBar;
    public int numGrenades;
    public int maxGrenades;
    public float throwVelocity;

    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            ThrowGrenade();
        }
    }

    public void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadeAsset, transform.position, transform.rotation, grenadeParentTransform);
        Rigidbody2D body = grenade.GetComponent<Rigidbody2D>();
        body.velocity = transform.right * throwVelocity;
        numGrenades--;
        UpdateGrenadeBar();
    }

    public void UpdateGrenadeBar()
    {
        grenadeBar.fillAmount = (float)numGrenades / maxGrenades;
    }
}
