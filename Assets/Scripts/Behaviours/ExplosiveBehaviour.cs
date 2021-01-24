using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // Cache
    private CircleCollider2D _proximityTrigger;
    private ExplosionStage _explosionStage;
    private CircleCollider2D _explosionCollider;
    private float _timeBeforeDestroyingParticles;
    private SpriteRenderer _radialView;
    private bool _blink;
    private float _blinkTime = 0.2f;
    
    public enum ExplosionType
    {
        TimeDetonated,
        RemotelyDetonated,
        ProximityTriggered,
    }

    private enum ExplosionStage
    {
        Waiting,
        FuseActivated,
        Exploding,
        Dead
    }

    // Start is called before the first frame update
    void Start()
    {
        _radialView = GetComponentsInChildren<SpriteRenderer>()[1];
        UpdateRadialView();
        //Init Some Vars
        _explosionCollider = GetComponent<CircleCollider2D>();
        _explosionCollider.radius = ExplosionRadius;
        _explosionCollider.enabled = false;
        
        _proximityTrigger = GetProximityTrigger();

        //Start Explosion
        if(Type == ExplosionType.TimeDetonated)
        {
            StartCoroutine(StartExplosion());
           
        } else if (Type == ExplosionType.ProximityTriggered)
        {
            _proximityTrigger.radius = ProximityActivationRadius;
        }

        if (Type != ExplosionType.ProximityTriggered)
        {
            _proximityTrigger.enabled = false;
        }
    }


    // Updates radial view radius
    private void UpdateRadialView()
    {
       
            Bounds b = _radialView.sprite.bounds;
            _radialView.gameObject.transform.localScale /= ((b.max - b.min).x / (ProximityActivationRadius*2)) * _radialView.gameObject.transform.localScale.x;

        
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
            case ExplosionStage.FuseActivated:
                // Changle look on fuse
                if (!_blink)
                    StartCoroutine(Blink());
                break;
            case ExplosionStage.Exploding:
                _blink = false;
                break;
            case ExplosionStage.Dead:
                break;
        }
    }

    //Coroutines
    private IEnumerator StartExplosion()
    {
        //Fuse is on waiting for explosion
        _explosionStage = ExplosionStage.FuseActivated;

        yield return new WaitForSecondsRealtime(FuseTime);
        _explosionStage = ExplosionStage.Exploding;
        SpawnParticles();
        _explosionCollider.enabled = true;

        //Explosion in progress waiting for it to end
        yield return new WaitForSecondsRealtime(ExplosionDuration);
        _explosionStage = ExplosionStage.Dead;
        StopParticles();
        _explosionCollider.enabled = false;
    }

    private IEnumerator Blink()
    {
        float t = 0;
        _blink = true;
        while (_blink)
        {
            Color start = _radialView.color;
            Color end = _radialView.color == Color.red ? new Color(1,0,0,0) : Color.red;
            for (int i = 0; i < 20; i++)
            {
                t = ((_blinkTime / 20.0f)*(i+1))/_blinkTime;
                yield return new WaitForSeconds( (_blinkTime / 20.0f));
                _radialView.color = Color.Lerp(start, end,t);
            }
        }
        
    }
    
    private IEnumerator DestroyParticles()
    {
        yield return new WaitForSecondsRealtime(_timeBeforeDestroyingParticles);
        Destroy(ExplosionParticles);
        // Destroying bomb
        Destroy(gameObject);
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

    //Could produce null pointer exception
    private CircleCollider2D GetProximityTrigger()
    {
        CircleCollider2D[] colliders = GetComponentsInChildren<CircleCollider2D>();
        
        foreach (CircleCollider2D collider in colliders)
        {
            if (collider != _explosionCollider)
                return collider;
        }

        return null;
    }

}
