using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBehaviour : MonoBehaviour
{
    public ExplosionType Type;
    public float ProximityActivationRadius;
    public float FuseTime;
    public float ExplosionRadius;
    public float ExplosionDuration;
    public float ExplosionStrength;

    public GameObject ExplosionParticles;
    public CircleCollider2D ProximityTrigger;

    private ExplosionStage _explosionStage;
    private CircleCollider2D _explosionCollider;
    private float _timeBeforeDestroyingParticles;

    public enum ExplosionType
    {
        TimeDetonated,
        RemotleyDetonated,
        ProximityTriggered,
    }

    private enum ExplosionStage
    {
        waiting,
        fuseActivated,
        exploding,
        dead
    }

    // Start is called before the first frame update
    void Start()
    {
        //Init Some Vars
        _explosionCollider = GetComponent<CircleCollider2D>();
        _explosionCollider.radius = ExplosionRadius;

        //Start Explosion
        if(Type == ExplosionType.TimeDetonated)
        {
            StartCoroutine(StartExplosion());
        } else if (Type == ExplosionType.ProximityTriggered)
        {
            ProximityTrigger.radius = ProximityActivationRadius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBombLook();
    }

    private void UpdateBombLook()
    {
        switch (_explosionStage)
        {
            case ExplosionStage.fuseActivated:
                break;
            case ExplosionStage.exploding:
                break;
            case ExplosionStage.dead:
                break;
        }
    }

    //Coroutines
    private IEnumerator StartExplosion()
    {
        //Fuse is on waiting for explosion
        _explosionStage = ExplosionStage.fuseActivated;

        yield return new WaitForSecondsRealtime(FuseTime);
        _explosionStage = ExplosionStage.exploding;
        SpawnParticles();
        _explosionCollider.enabled = true;

        //Explosion in progress waiting for it to end
        yield return new WaitForSecondsRealtime(ExplosionDuration);
        _explosionStage = ExplosionStage.dead;
        StopParticles();
        _explosionCollider.enabled = false;
    }

    private IEnumerator DestroyParticles()
    {
        yield return new WaitForSecondsRealtime(_timeBeforeDestroyingParticles);
        Destroy(ExplosionParticles);
    }

    private void SpawnParticles()
    {
        ExplosionParticles = Instantiate(ExplosionParticles, transform);
        ExplosionParticles.transform.parent = transform;
    }

    private void StopParticles()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }

        StartCoroutine(DestroyParticles());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // Delete if rocket
            RocketMovement rocket = collision.gameObject.GetComponentInParent<RocketMovement>();
            if (rocket)
            {
                Vector2 direction = (collision.gameObject.transform.position - transform.position);
                rocket.InvokeDeath(direction*ExplosionStrength);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void StartExplotion()
    {
        StartCoroutine(StartExplosion());
    }
}
