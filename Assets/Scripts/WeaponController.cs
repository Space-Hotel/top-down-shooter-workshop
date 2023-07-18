using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletAsset;
    public GameObject bulletContainer;
    public GameObject flash;
    public float bulletVelocity = 5f;
    public float fireSpeed = 5f;
    public float reloadTime = 1.5f;
    public int ammoPerClip = 30;
    public int ammo = 30;
    private bool readyToShoot = true;
    private bool isReloading = false;
    private bool isFlash = false;

    private void Update()
    {
        if (isFlash) isFlash = false;
        if (Input.GetMouseButton(0) && readyToShoot && !isReloading)
        {
            Fire();
            StartCoroutine(ShootCooldownRoutine());
            isFlash = true;
        }
        flash.SetActive(isFlash);

        if ((ammo <= 0 && !isReloading) || Input.GetKeyDown("r"))
        {
            StartCoroutine(ReloadingRoutine());
        }
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletAsset, transform.position, transform.rotation, bulletContainer.transform);
        Rigidbody2D body = bullet.GetComponent<Rigidbody2D>();
        body.velocity = transform.right * bulletVelocity;
        ammo--;
    }

    public IEnumerator ShootCooldownRoutine()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(1f / fireSpeed);
        readyToShoot = true;
    }

    public IEnumerator ReloadingRoutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo = ammoPerClip;
        isReloading = false;
    }
}

