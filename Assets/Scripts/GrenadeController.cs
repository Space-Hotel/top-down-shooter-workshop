using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrenadeController : MonoBehaviour
{
    public Animator explosionAnimator;
    public SpriteRenderer blastAsset;
    public GameObject light;
    public float grenadeDelay = 3f;
    public float damage = 80f;

    private AudioSource grenadeExplosion;
    private SpriteRenderer renderer;
    private Rigidbody2D body;
    private CircleCollider2D collider;
    private CircleCollider2D blastCollider;

    List<PlayerController> affectedPlayers = new List<PlayerController>();
    List<ZombieController> affectedEnemies = new List<ZombieController>();

    void Start()
    {
        grenadeExplosion = GetComponents<AudioSource>()[1];
        renderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
        collider = colliders[0];
        blastCollider = colliders[1];
        StartCoroutine(GrenadeCookingRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            affectedPlayers.Add(collidedObject.GetComponent<PlayerController>());
        }
        else if (collidedObject.CompareTag("Enemy"))
        {
            affectedEnemies.Add(collidedObject.GetComponent<ZombieController>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player"))
        {
            affectedPlayers.Remove(collidedObject.GetComponent<PlayerController>());
        }
        else if (collidedObject.CompareTag("Enemy"))
        {
            affectedEnemies.Remove(collidedObject.GetComponent<ZombieController>());
        }
    }

    private IEnumerator GrenadeCookingRoutine()
    {
        yield return new WaitForSeconds(grenadeDelay);
        grenadeExplosion.Play();
        explosionAnimator.enabled = true;
        renderer.enabled = false;
        Destroy(body);
        collider.enabled = false;
        light.SetActive(false);
        blastAsset.enabled = true;
        for (int i = 0; i < affectedPlayers.Count; i++)
        {
            affectedPlayers[i].health -= damage;
            affectedPlayers[i].UpdateHealth();
        }
        for (int i = 0; i < affectedEnemies.Count; i++)
        {
            affectedEnemies[i].health -= damage;
        }
    }
}
