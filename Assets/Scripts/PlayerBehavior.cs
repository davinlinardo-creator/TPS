using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float RotateSpeed = 95f;
    public float JumpVelocity = 5f;
    private float _vInput;
    private float _hInput;
    private bool _isJumping;
    private Rigidbody _rb;
    public bool isGrounded = true;
    public float GroundcheckRadius = 0.3f;
    public LayerMask GroundLayer;
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    private bool _isShooting;
    public GameBehavior GameManager;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        GameManager = GameObject.Find("Game manager").GetComponent<GameBehavior>();
    }

    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;
        _isJumping |= Input.GetKeyDown(KeyCode.Space);
        _isShooting |= Input.GetKeyDown(KeyCode.Mouse0);

        isGrounded = Physics.CheckSphere(transform.position, GroundcheckRadius, GroundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _isJumping = true;
        }
    }

    void FixedUpdate()
    {
        // Gerak maju mundur
        _rb.MovePosition(transform.position + transform.forward * _vInput * Time.fixedDeltaTime);

        // Rotasi kiri kanan
        Quaternion angleRot = Quaternion.Euler(Vector3.up * _hInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);

        // Lompat
        if (_isJumping)
        {
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
            _isJumping = false;
        }

        if (_isShooting)
        {
            Vector3 spawnPos = transform.position + transform.forward * 1f;
            GameObject newBullet = Instantiate(Bullet, spawnPos, this.transform.rotation);
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.linearVelocity = this.transform.forward * BulletSpeed;
            _isShooting = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, GroundcheckRadius);
    }

}