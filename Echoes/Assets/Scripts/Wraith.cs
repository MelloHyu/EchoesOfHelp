using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wraith : MonoBehaviour
{
    public event EventHandler OnTriggeredWraith;

    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    [SerializeField] private Transform target;
    private Vector2 moveDirection;
    [SerializeField] private bool chaseSequenceBegan = false;

    [SerializeField] private Animator anim;
    [SerializeField] private AudioLoudnessDetection detector;
    [SerializeField] private float loudnessSensibility = 100f;
    [SerializeField] private float chaseThreshold = 0.1f;
    [SerializeField] private float audioTimerLimit = 2f;
    [SerializeField] private float maxChaseTimer = 20f;
    private float chaseTimer = 0f;
    private float timer = 0f;
    private Vector2 lastDir;
    private bool isRunning;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        OnTriggeredWraith += Wraith_OnTriggeredWraith;
        timer = 0f;
        chaseTimer = 0f;
        chaseSequenceBegan = false;

    }

    private void Wraith_OnTriggeredWraith(object sender, EventArgs e)
    {
        chaseTimer = 0f;
        Debug.Log("Wraith has started chasing");
        chaseSequenceBegan = true;
    }

    private void Update()
    {
        Debug.Log(chaseTimer);
        if(target && chaseSequenceBegan && chaseTimer<=maxChaseTimer)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            moveDirection = dir;
            chaseTimer += Time.deltaTime;
        }

        if(maxChaseTimer <= chaseTimer)
        {
            chaseSequenceBegan = false;
            chaseTimer = 0f;
        }

        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (IsAboveLoudnessThreshold(loudness))
        {
            timer += Time.deltaTime;
        }
        
        if (HasTriggeredChaseThreshold(loudness))
        {
            OnTriggeredWraith?.Invoke(this, EventArgs.Empty);
        }
        //Debug.Log(timer);
    }

    private void FixedUpdate()
    {
        if(target && chaseSequenceBegan)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            if(moveDirection.sqrMagnitude>0)
            {
                lastDir = moveDirection;
            }
            isRunning = rb.velocity.sqrMagnitude > 0;

        }

        Animate();
    }
    private bool HasTriggeredChaseThreshold(float loudness)
    {
        if (IsAboveLoudnessThreshold(loudness) && HasReachedTimeLimit())
        {
            timer = 0f;
            return true;
        }

        else return false;
    }

    private bool HasReachedTimeLimit()
    {
        if (timer >= audioTimerLimit)
        {
            timer = 0f;
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
        anim.SetFloat("AnimX", moveDirection.x);
        anim.SetFloat("AnimY", moveDirection.y);
        anim.SetBool("IsRunning", isRunning);
    }

}
