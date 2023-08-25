using System;
using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
   /* [SerializeField] private AlienEgg _eggPrefab;
    [SerializeField] private int _hitsToDestroy = 3;
    private int _hitCount;
    [SerializeField] private GameObject _destructionParticle;
    [SerializeField, Range(1, 100)] private int _eggSpawnRate = 50;
    [SerializeField] private ResourceType _asteroidResourceType;
    [SerializeField] private float _explosionForce = 300f;
    [SerializeField] private float _explosionRadius = 5f;

    public int CurrentHealth() => _hitsToDestroy - _hitCount;
    public Vector2 SpawnLocation;
    public int spawnPriority = -1;
    [HideInInspector] public AsteroidPattern ParentAsteroidPattern;
    Rigidbody2D rb2d;

    public static Action<Asteroid> OnAsteroidAttacked;

    public bool Activate
    {
        set
        {
            if (value)
            {
                AsteroidGrid.ActiveAsteroidCount++;
                gameObject.SetActive(true);
            }
            else
            {
                AsteroidGrid.ActiveAsteroidCount--;
                gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = gameObject.AddComponent<Rigidbody2D>();
        }
        rb2d.gravityScale = 0f;
    }

    private void Start()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.DamageCity(); // Call the DamageCity method from GameManager script

            ExplodeToDeath();
            AkSoundEngine.PostEvent("Play_Pay_Attention", gameObject);
        }
        *//*else if (collision.gameObject.GetComponent<Asteroid>() != null && spawnPriority > collision.gameObject.GetComponent<Asteroid>().spawnPriority)
        {
            Destroy(gameObject);
        }*//*
    }

    public int deltaHarvest = 1;

    public void ChangeHealth(int value)
    {
        _hitCount -= value;

        OnAsteroidAttacked?.Invoke(this);

        if (_hitCount >= _hitsToDestroy)
        {
            //A chance to drop sharpnel
            if(UnityEngine.Random.Range(0,5) == 1)
                ResourceParticleController.Instance.SpawnResource(transform.position, ResourceType.SHRAPNEL);

            if (UnityEngine.Random.Range(0, 100) <= _eggSpawnRate)
                AlienManager.Instance.SpawnEggAtPosition(_eggPrefab, transform.position);

            ExplodeToDeath();

        }
    }

    public void ExplodeToDeath()
    {
        Explode();
        Instantiate(_destructionParticle, transform.position, Quaternion.identity);
        AsteroidGrid.OnAsteroidDestroyed?.Invoke(this, ParentAsteroidPattern);
        ParentAsteroidPattern.ReportChildDestroyed();
        Destroy(gameObject);
    }

    public void Harvest(int _damage)
    {
        _hitCount += _damage;
        if (_hitCount >= _hitsToDestroy)
        {
            Instantiate(_destructionParticle, transform.position, Quaternion.identity);
            ResourceParticleController.Instance.SpawnResource(transform.position, _asteroidResourceType);
            Explode();
            AsteroidGrid.OnAsteroidDestroyed?.Invoke(this, ParentAsteroidPattern);
            ParentAsteroidPattern.ReportChildDestroyed();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Asteroid"))
            {
                Rigidbody2D rb2d = collider.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    Vector2 direction = collider.transform.position - transform.position;
                    CameraShake.instance.shakeCamera(5f, .2f);
                    rb2d.AddForce(direction.normalized * _explosionForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
    public void ChangeVelocity(float factor)
    {

        rb2d.velocity *= factor;
    }

    public void ApplyForce(Vector2 force)
    {

    }

    public void GetExplode()
    {
        Instantiate(_destructionParticle, transform.position, Quaternion.identity);
        ResourceParticleController.Instance.SpawnResource(transform.position, _asteroidResourceType);
        Explode();
        OnDestroy();
    }

    private void OnDestroy()
    {

    }

    public void ApplyForce(Vector2 direction, float lockDuration)
    {
        Vector2 origin = rb2d.velocity;
        rb2d.velocity = direction;
        if (gameObject.activeInHierarchy) StartCoroutine(ForceHandler(origin,lockDuration));
    }
    private IEnumerator ForceHandler(Vector2 originalDirection,float duration)
    {
        yield return new WaitForSeconds(duration);
        rb2d.velocity = originalDirection;
    }*/
}
