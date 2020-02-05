using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float groundDistance = 0.4f;

    [SerializeField] private GameObject playerChar = null;

    [SerializeField] private Transform groundCheck = null;
    [SerializeField] private Transform camQuat;

    [SerializeField] private LayerMask groundMask = 0;

    private CharacterController _controller = null;
    private Animator _animator = null;
    private Vector3 _velocity;

    private bool _isGrounded;

    private void Awake() 
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start() 
    {
        _animator = playerChar.GetComponent<Animator>();
    }

    private void Update() 
    {
        Movement();
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;
        _controller.SimpleMove(Vector3.ClampMagnitude(move, 1.0f) * speed);

        _animator.SetFloat("inputY", z);
        _animator.SetFloat("inputX", x);

        JumpMovement();
    }

    private void JumpMovement()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && _isGrounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

        _animator.SetBool("isJumping", _isGrounded);

        _velocity.y += gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }
}
