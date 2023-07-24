using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image healthbar;
    public float maxHealth = 100f;
    public float health = 100f;
    public float aimOffset = 0f;
    public float moveSpeed = 0.1f;

    private AudioSource walkSound;
    private bool isWalking = false;

    private Rigidbody2D body;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        walkSound = GetComponent<AudioSource>();
    }
    void Update()
    {
        Aim();
        Move();
    }

    public void Aim()
    {
        Vector2 direction = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        rotation *= Quaternion.Euler(0f, 0f, -90f - aimOffset);
        transform.rotation = rotation;
    }

    public void Move()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        moveDirection *= Mathf.Lerp(0f, moveSpeed, Time.deltaTime);
        body.position += moveDirection;
        if (moveDirection != Vector2.zero && !isWalking)
        {
            isWalking = true;
            walkSound.Play();
        }
        else if (moveDirection == Vector2.zero && isWalking)
        {
            isWalking = false;
            walkSound.Stop();
        }
    }

    public void UpdateHealth()
    {
        healthbar.fillAmount = health / maxHealth;
    }
}
