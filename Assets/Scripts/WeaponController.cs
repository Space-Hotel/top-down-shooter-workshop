using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [Header("Assets")]
    public GameObject bulletAsset;
    public GameObject bulletContainer;
    public GameObject flash;
    public Image ammoBar;
    [Header("Parameters")]
    [Range(5f, 100f)]
    public float bulletVelocity = 5f;
    public float fireSpeed = 5f;
    [Tooltip("Time it takes to reload the gun from ammo:0 to ammo: ammoPerClip.")]
    public float reloadTime = 1.5f;
    [Header("Ammo")]
    public int ammoPerClip = 30;
    public int ammo = 30;
    public bool readyToShoot = true;
    private bool isReloading = false;
    private bool isFlash = false;
    private AudioSource fireSound;
    private AudioSource reloadSound;

    public void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        fireSound = audioSources[0];
        reloadSound = audioSources[1];
    }

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
        fireSound.Play();
        UpdateAmmoBar();
    }

    public void UpdateAmmoBar()
    {
        ammoBar.fillAmount = (float)ammo / ammoPerClip;
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
        reloadSound.Play();
        yield return new WaitForSeconds(reloadTime);
        ammo = ammoPerClip;
        UpdateAmmoBar();
        reloadSound.Stop();
        isReloading = false;
    }
}

