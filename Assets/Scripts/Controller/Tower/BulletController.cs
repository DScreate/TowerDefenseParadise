using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : PoolObject {

    public int damage;

    public float lifetime = 2f;

    public TowerController tower;

    private float creationTime;

    public Quaternion baseRot;

    private GameObject _parent;
    private Rigidbody _rbody;

    public GameObject parent
    {
        get
        {
            if(_parent == null)
                _parent = transform.parent.gameObject;

            return _parent;
        }

        set { _parent = value; }
    }

    public Rigidbody rbody
    {
        get
        {
            if (_rbody == null)
                _rbody = GetComponent<Rigidbody>();

            return _rbody;
        }

        set { _rbody = value; }
    }

    private void Awake()
    {
        //baseRot = Quaternion.Euler(transform.rotation.eulerAngles);

        //Debug.Log(transform.rotation.eulerAngles);
        //Debug.Log(baseRot.eulerAngles);

        if (parent == null)
            parent = transform.parent.gameObject;

        if (rbody == null)
            rbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Time.time >= creationTime + lifetime || Vector3.Distance(tower.transform.position, transform.position) * 1.6f > tower.tower.range)
            Destroy();

        //transform.rotation = Quaternion.LookRotation(rbody.velocity);
    }

    private void OnEnable()
    {
        creationTime = Time.time;

        ResetRbody();
    }

    private void OnDisable()
    {
        ResetRbody();
    }

    private void OnCollisionStay(Collision other)
    {
        //Debug.Log("Collision " + other.gameObject.name, other.gameObject);

        Transform otherParent = other.transform.parent;

        if (otherParent != null)
        {
            EnemyController enemy = otherParent.GetComponent<EnemyController>();

            if (enemy != null)
            {
                //Debug.Log("Collision Enemy");

                enemy.TakeDamage(damage);

                Destroy();
            }
            else
            {
                //Destroy(gameObject);
            }
        }
    }

    public override void Destroy()
    {
        //ResetRbody();

        parent.SetActive(false);

        PoolManager.EnqueueObject(parent);
    }

    public override void OnObjectReuse()
    {
        base.OnObjectReuse();

        //ResetRbody();
    }

    public void ResetRbody()
    {
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;
        //transform.SetPositionAndRotation(parent.transform.position, baseRot * parent.transform.rotation);
        transform.position = parent.transform.position;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
    }

}


// Below is my bullet controller from the void. It is of no use here but some code may be salvaged later.
/*
public class BulletController : SectorObjMover {

    [Space(4)]
    [Header("Bullet Settings")]
    //[SerializeField]
    //private int damage = 10;
    //[SerializeField]
    //private DamageTypes damageType;
    [SerializeField]
    private Damage damage;
    [SerializeField]
    private float destroyDelay = 2.0f;
    [SerializeField]
    private float bulletForce = 1000;
    [Space(4)]
    [Header("Explosion Settings")]
    [SerializeField]
    private Transform explosion;
    [SerializeField]
    private int explosionDmg = -1;
    [Space(4)]
    [Header("Graphics Settings")]
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    //public GameObject[] trailParticles;

    [Space(4)]
    [Header("Editor Settings")]
    [SerializeField]
    private GameObject owner;
    private Transform parent;
    private ManualTrail trail;

    //private Vector3 impactNormal; //Used to rotate impactparticle.
    private GameObject particle;
    private GameObject muzzle;
    private GameObject impact;

    private bool hasCollided = false;
    private Rigidbody2D _rigidBody2D;

    private Coroutine _destroyCoroutine;

    private void Awake()
    {
        if (_rigidBody2D == null)
            _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void OnStart()
    {
        gameObject.SetActive(true);
        _rigidBody2D.velocity = Vector2.zero;
        hasCollided = false;
        Random.InitState(Mathf.RoundToInt(Time.time * 1048576) + GetInstanceID());

        //Quaternion rot = Quaternion.Euler(-90, 0, 0);

        //particle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        particle = PoolManager.poolManager.ReuseObject(projectileParticle, transform.position, transform.rotation, false, false);
        particle.transform.parent = transform;
        particle.GetComponent<AudioSource>().pitch = (Mathf.Clamp(30f / bulletForce + 0.1f, 0.5f, 1.5f) + Mathf.Clamp(500f / damage.amount + 0.1f, 0.5f, 1.5f)) / 2f * Random.Range(.95f, 1.05f);
        particle.SetActive(true);

        Rigidbody2D bulletBody = GetComponent<Rigidbody2D>();
        bulletBody.AddForce(transform.up * bulletForce);
        //Destroy(gameObject, destroyDelay);
        trail = transform.GetComponentInChildren<ManualTrail>();
        //parent = transform.parent;
        parent = transform.parent = GameManager.bulletManager.transform;
        SectorController.sectorController.AddObjToSector(gameObject);

        if (muzzleParticle != null)
        {
            //muzzle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            muzzle = PoolManager.poolManager.ReuseObject(muzzleParticle, transform.position, transform.rotation, false, false);
            muzzle.transform.rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
            //muzzle.transform.parent = GameManager.bulletManager.transform;
            //Destroy(muzzle, 1.5f); // Lifetime of muzzle effect.
            BulletManager.bulletManager.StartCoroutine(DeactivateParticles(muzzle, 1.5f));
            muzzle.SetActive(true);
        }

        Light light = transform.GetComponentInChildren<Light>();

        if (!SettingsController.bulletLights && light != null)
            light.gameObject.SetActive(false);

        //Invoke("Destroy", destroyDelay);
        _destroyCoroutine = StartCoroutine(DestroyAfterTime(destroyDelay));
    }

    void Update()
    {
        Vector2 moveDirection = GetComponent<Rigidbody2D>().velocity;

        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(-moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void Destroy()
    {
        if (gameObject.activeSelf)
        {
            hasCollided = true;
            //impact = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            //impact = Instantiate(impactParticle, transform.position, transform.rotation) as GameObject;
            impact = PoolManager.poolManager.ReuseObject(impactParticle, transform.position, transform.rotation, false, false);
            AudioSource audio = impact.GetComponent<AudioSource>();
            if (audio != null)
                audio.pitch = (Mathf.Clamp(50f / bulletForce + 0.1f, 0.5f, 1.5f) + Mathf.Clamp(250f / damage.amount + 0.1f, 0.5f, 1.5f)) / 2f * Random.Range(.9f, 1.1f);

            impact.SetActive(true);

            //impact.transform.parent = GameManager.bulletManager.transform;
            //Destroy(impact, 5f);
            BulletManager.bulletManager.StartCoroutine(DeactivateParticles(impact, 5f));

            //Destroy(particle, 3f);
            BulletManager.bulletManager.StartCoroutine(DeactivateParticles(particle, 3f));

            if (explosion != null && explosionDmg > -1)
            {
                Random.InitState(Mathf.RoundToInt(Time.time * 1048576) + GetInstanceID());
                Transform temp = SpriteHelpers.AddPrefab(explosion, transform.position, Random.value * 360);
                temp.GetComponent<ExplosionController>().SetDamage(explosionDmg);
                temp.GetComponent<ExplosionController>().SetOwner(owner);
            }
            if (SectorController.sectorController != null)
                SectorController.sectorController.RemoveObjFromSector(gameObject);

            gameObject.SetActive(false);
            PoolManager.EnqueueObject(gameObject);
            parent = transform.parent = PoolManager.GetPoolHolder(gameObject).transform;
            //SectorController.sectorController.RemoveObjFromSector(gameObject);
            muzzle = null;
            impact = null;
            particle = null;
            //StartCoroutine(DeactivateParticles(gameObject, 7f));
        }
    }

    public void SetDamage(int dmg)
    {
        damage.amount = dmg;
    }

    public void SetForce(float frc)
    {
        bulletForce = frc;
    }

    public void SetExplosion(Transform explo, int dmg)
    {
        explosion = explo;
        explosionDmg = dmg;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }

    public GameObject GetOwner() { return owner; }

    public void SetMass(float mass)
    {
        Rigidbody2D bulletBody = GetComponent<Rigidbody2D>();
        bulletBody.mass = mass;
    }

    public void SetLifeTimer(float delay)
    {
        destroyDelay = delay;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ship>() != null && !hasCollided)
        {
            if (collision.gameObject != owner && collision.gameObject.transform.parent.gameObject != owner)
            {
                hasCollided = true;

                collision.gameObject.GetComponent<Ship>().TakeDamage(damage, owner);


                ReleaseTrail();

                Destroy();
            }
        }
    }

    public void SetUpBullet(Transform parent, GameObject owner, Transform explosion, float bulletLife, float initialForce, float mass, Damage damage, int explosionDmg)
    {
        destroyDelay = bulletLife;
        this.parent = parent;
        bulletForce = initialForce;
        this.damage = damage;
        SetExplosion(explosion, explosionDmg);
        this.owner = owner;
        SetMass(mass);
    }

    private void ReleaseTrail()
    {
        if (trail != null)
        {
            trail.transform.parent = parent;
            trail.DestroyAfter(.5f);
            SectorController.sectorController.AddObjToSector(trail.gameObject);
        }
    }

    public override void OnSectorMove(Vector2 posChange)
    {
        if (trail != null)
        {
            trail.OnSectorMove(posChange);
        }
    }

    private void OnDisable()
    {
        if (_destroyCoroutine != null)
            StopCoroutine(_destroyCoroutine);
    }

    IEnumerator DeactivateParticles(GameObject particle, float time)
    {
        yield return new WaitForSeconds(time);


        if (particle.activeSelf)
        {
            particle.SetActive(false);
            PoolManager.EnqueueObject(particle);
            particle.transform.parent = PoolManager.GetPoolHolder(particle.gameObject).transform;
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy();
    }

}*/
