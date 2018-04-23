using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyController : MonoBehaviour {

    //public Camera cam;

    public NavMeshAgent agent;

    public Transform goalTransform;
    public GameObject soundHolder;

    public int health = 100;
    public int creditValue = 100;

    public AudioClip[] deathSounds;
    
    NavMeshPath path;

    float nextPathUpdate;

    public AudioMixerGroup mixerGroup;

    private bool dead = false;

    void Start ()
    {
        path = new NavMeshPath();

        health = 150 * GameManager.chapter;

        soundHolder = GameManager.soundHolder;
    }



	void Update ()
    {
        if (Time.time >= nextPathUpdate && goalTransform != null)
        {
            SetPathToGoal(goalTransform.position);

            nextPathUpdate = Time.time + 1f;
        }

        path = agent.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);

        CheckPath(agent.path);
    }

    public bool CheckPath(NavMeshPath path)
    {
        bool ret = path.status != NavMeshPathStatus.PathPartial;

        if (!ret)
        {
            Debug.Log("Destination Impossible");
        }

        return ret;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0 && !dead)
            Die();
    }

    public void Die(bool remove = true, bool giveCredits = true, bool playSound = true)
    {
        dead = true;

        if(remove)
            SpawnController.spawnController.enemies.Remove(this);

        if(giveCredits)
            GameManager.AddMoney(creditValue);
        
        if(playSound)
            StartSound(deathSounds[GameManager.GetRandomInt(0, deathSounds.Length)]);
        
        Destroy(gameObject);
    }

    public void SetPathToGoal(Vector3 goal)
    {
        agent.SetDestination(goal);
    }
    
    public void StartSound(AudioClip clip)
    {
        AudioSource audioSource = soundHolder.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.Play();

        StartCoroutine(DestroyAudioSource(audioSource, 3f));
    }

    private IEnumerator DestroyAudioSource(AudioSource audioSource, float wait)
    {
        yield return new WaitForSeconds(wait);

        //Debug.Log("Destroy Audio Source");
        Destroy(audioSource);
    }
}
