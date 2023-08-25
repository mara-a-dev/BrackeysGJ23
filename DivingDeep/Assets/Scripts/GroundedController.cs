using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedController : MonoBehaviour
{

    private PlayerController _playerController;

    private Animator _mechAnimator;
    private Rigidbody2D _rb;
    private SpriteRenderer _rend;
    private float _currentSpeed = 70f;
    public bool IsFiringEnabled { get; set; } = true; // Add the public boolean variable


    [SerializeField] private GameObject _laserBeamPrefab;
    [SerializeField] private GameObject _muzzleFlashPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] public float _fireRate;

    private float _timeSinceLastShot;


    public void SetPlayerController(PlayerController controller)
    {
        _playerController = controller;
        _playerController.OnPrimaryFirePerformed += FireLaser;
    }

    // Start is called before the first frame update
    void Start()
    {
        _mechAnimator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisuals();
        IsFiringEnabled = (GameManager.Instance.CurrentState != GameState.UPGRADING);
        _timeSinceLastShot += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        ShipMovement();
    }

    private void ShipMovement()
    {
        Vector2 force = new(_playerController.MoveInput.x * Time.fixedDeltaTime, 0f);
        force.Normalize();

        _rb.AddForce(force * _currentSpeed);
    }
    private void FireLaser()
    {
        if (!IsFiringEnabled || _timeSinceLastShot < _fireRate) return;
        Instantiate(_laserBeamPrefab, _firePoint.position, _firePoint.rotation);
        var muzzle = Instantiate(_muzzleFlashPrefab, _firePoint.position, _firePoint.rotation, _firePoint);
        _timeSinceLastShot = 0;
        Destroy(muzzle, .1f);
      //  AkSoundEngine.PostEvent("Play_SFX_Ship_Shoot", gameObject);
    }

    private void PlayEvent()
    {
        //AkSoundEngine.PostEvent("Play_SFX_Ship_Walk", gameObject);
    }
    private void UpdateVisuals()
    {
        _rend.flipX = _playerController.MoveInput.x < 0f;
        _mechAnimator.SetBool("IsMoving", _playerController.MoveInput.x != 0f); 
    }

}
