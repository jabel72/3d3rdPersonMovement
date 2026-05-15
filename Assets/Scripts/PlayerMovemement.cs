// https://www.youtube.com/watch?v=Cd2Erk_bsRY
//https://docs.unity3d.com/Packages/com.unity.inputsystem@1.19/manual/Videos.html


using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovemement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputActionAsset inputActions;

    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_jumpAction;
    
    private Vector2 m_moveAmt;
    private Vector2 m_lookAmt;
    //private Animator m_animator;
    private Rigidbody m_rigidbody;
    public float walkSpeed ;
    public float rotateSpeed ;
    public float jumpSpeed ;
    // Ground distance check for jumping
    private float groundCheckDistance = 0.1f;
    
    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();    
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();    
    }

    private void Awake()
    {
        m_moveAction = inputActions.FindAction("Player/Move");
        m_lookAction = inputActions.FindAction("Player/Look");
        m_jumpAction = inputActions.FindAction("Player/Jump");
        
        
        //m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();
        m_lookAmt = m_lookAction.ReadValue<Vector2>();
        

        if (m_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    
    }
    
    void FixedUpdate()
    {
        Walking();
        Rotating();
       
    }

    public void Jump()
    {        
        if (IsGrounded())
        {
            m_rigidbody.AddForce (Vector3.up * jumpSpeed, ForceMode.Impulse);
            //m_animator.SetTrigger("Jump");
        }
    }

    public void Walking()
    {
        //m_animator.SetFloat("Speed", m_moveAmt.y);
        m_rigidbody.MovePosition(m_rigidbody.position + transform.forward * m_moveAmt.y * walkSpeed * Time.deltaTime +transform.right * m_moveAmt.x * walkSpeed * Time.deltaTime);
        
    }

    private void Rotating()
    {
        //if (m_moveAmt.y != 0)
        {
            float rotationAmount = m_lookAmt.x * rotateSpeed * Time.deltaTime ;
            Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
            m_rigidbody.MoveRotation(m_rigidbody.rotation * deltaRotation);
            
        }
    }

    private bool IsGrounded ()
    {
        Debug.Log("Is Grounded: " + Physics.Raycast(transform.position, Vector3.down, groundCheckDistance));
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    private void OnDrawGizmos()
  {
      Gizmos.color = IsGrounded() ? Color.green : Color.red;
      Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
  }
    
}