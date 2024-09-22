using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Rigidbody2D playerRB;
    [SerializeField] private Animator anim;
    [SerializeField] private float movementSpeed = 10f;
    private Vector2 lastDir;
    private bool isRunning = false;
    [SerializeField] private AudioSource footsteps;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(isRunning)
        {
            PlayFootsteps();
        }
    }
    private void OnEnable()
    {
        playerInputActions.Enable();
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = playerInputActions.Movement.Move.ReadValue<Vector2>();
        if(moveInput.sqrMagnitude>0)
        {
            lastDir = moveInput;
        }
        playerRB.velocity = moveInput * movementSpeed;
        isRunning = playerRB.velocity.sqrMagnitude > 0;
        Animate();
    }

    private void Animate()
    {
        anim.SetFloat("AnimMoveX", lastDir.x);
        anim.SetFloat("AnimMoveY", lastDir.y);
        anim.SetBool("IsRunning", isRunning);
    }

    private void PlayFootsteps()
    {
        footsteps.Play();
    }

}
