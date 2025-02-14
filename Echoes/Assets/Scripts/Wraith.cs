using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wraith : MonoBehaviour
{

    [SerializeField] private AudioSource ghostIncomingScream;
    [SerializeField] private Transform distFromPlayer;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform target;
    [SerializeField] private bool chaseSequenceBegan = false;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioLoudnessDetection detector;
    [SerializeField] private float loudnessSensibility = 100f;
    [SerializeField] private float chaseThreshold = 0.1f;
    [SerializeField] private float audioTimerLimit = 2f;
    [SerializeField] private float maxChaseTimer = 20f;


    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 idlePosition;
    private float chaseTimerCountdown = 0f;
    private float beginChaseTimerCountdown = 0f;
    private Vector2 animationLastIdleDirection;
    private bool isRunning;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
        beginChaseTimerCountdown = 0f;
        chaseTimerCountdown = 0f;
        chaseSequenceBegan = false;

    }

    private void WraithTrigger()
    {
        chaseTimerCountdown = 0f;
        
        Debug.Log("Wraith has started chasing");
        chaseSequenceBegan = true;
    }

    private void Update()
    {
        //Debug.Log(chaseTimer);
        if(target && chaseSequenceBegan && chaseTimerCountdown<=maxChaseTimer)
        {
            
            ChasePlayer();
        }

        if(maxChaseTimer <= chaseTimerCountdown)
        {
            chaseSequenceBegan = false;
            chaseTimerCountdown = 0f;
        }

        if(!chaseSequenceBegan)
        {
            Vector2 Idledir = (distFromPlayer.position - transform.position).normalized;
            idlePosition = Idledir;
        }

        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (IsAboveLoudnessThreshold(loudness))
        {
            beginChaseTimerCountdown += Time.deltaTime;
        }
        
        if (HasTriggeredChaseThreshold(loudness))
        {
            ghostIncomingScream.Play();
            WraithTrigger();
        }
        Debug.Log(beginChaseTimerCountdown);
    }

    private void FixedUpdate()
    {
        if(target && chaseSequenceBegan)
        {
           
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            if(moveDirection.sqrMagnitude>0)
            {
                animationLastIdleDirection = moveDirection;
            }
            isRunning = rb.velocity.sqrMagnitude > 0;

        }
        else
        {
            rb.velocity = new Vector2(idlePosition.x, idlePosition.y) * moveSpeed;
            isRunning = rb.velocity.sqrMagnitude > 0;
        }
       
        Animate();
    }
    private bool HasTriggeredChaseThreshold(float loudness)
    {
        if (IsAboveLoudnessThreshold(loudness) && HasReachedTimeLimit())
        {
            beginChaseTimerCountdown = 0f;
            return true;
        }

        else return false;
    }

    private bool HasReachedTimeLimit()
    {
        if (beginChaseTimerCountdown >= audioTimerLimit)
        {
            beginChaseTimerCountdown = 0f;
            return true;
        }
        else return false;
    }

    private bool IsAboveLoudnessThreshold(float loudness)
    {
        if (loudness > chaseThreshold)
        {
            return true;
        }
        else return false;
    }

    private void Animate()
    {
        anim.SetFloat("AnimX", animationLastIdleDirection.x);
        anim.SetFloat("AnimY", animationLastIdleDirection.y);
        anim.SetBool("IsRunning", isRunning);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.RestartLevel();
        }

    }

    private void ChasePlayer()
    {

        Vector2 dir = (target.position - transform.position).normalized;
        moveDirection = dir;
        chaseTimerCountdown += Time.deltaTime;
    }

}
