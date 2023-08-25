using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    public static ShipController instance;
    private PlayerController _playerController;
    private Rigidbody2D _rb;

    [SerializeField] private Transform _cameraTargetTransform;
    private SpriteRenderer _spriteRenderer;


    [Header("Weapons")]
   /* [SerializeField] private WeaponType _currentWeapon = WeaponType.LaserBeam;
    [SerializeField] private WeaponType _secondWeapon = WeaponType.GravityBeam;*/
    // [SerializeField] BulletCountBar countBar;

    private int _currentBullet;
    private int _secondBullet;
  //  [SerializeField] private int bulletCap;

 /*   private Dictionary<WeaponType, float> weaponCooldowns = new Dictionary<WeaponType, float>();
    private Dictionary<WeaponType, float> timeSinceLastWeapon = new Dictionary<WeaponType, float>();*/
    //[SerializeField] private VFXScript weaponChangeVFX;

    [Header("Tag Beam")]
  /*  [SerializeField] private GameObject _laserBeamPrefab;
    [SerializeField] private GameObject _muzzleFlashPrefab;
    [SerializeField] private Transform _firePoint;

    [SerializeField] public float _fireRate;

    [SerializeField] public bool IsFiringEnabled { get; set; } = true; // Add the public boolean variable*/
    public TrailRenderer shipTail1;
    public TrailRenderer shipTail2;
    [SerializeField] GameObject _shipThrust;
    public float _currentSpeed = 40f;
    private float _timeSinceLastLaser;
    private float _dashForce = 700;
    public bool upgradedBullets = false;

    [Header("Tag Beam")]
  /*  [SerializeField] private GameObject _tagBeamPrefab;
    [SerializeField] private GameObject _muzzleTagFlashPrefab;*/

    private bool _isFiringTag = false;
    private GameObject _currentTag;

    internal bool _canLaserTag = false;

    [Header("Dash Stats")]
    [SerializeField] GameObject _dashEffect;
    [SerializeField] float _maxDashDistance = 5;
    [SerializeField] float _dashDuration = 0.275f;
    [SerializeField] float _dashCoolDown = 1;
    public static bool canDash;

    [Header("Crash Charge")]
   // [SerializeField] VFXScript crashVFX;
    [SerializeField] GameObject chargeVFX;
    [SerializeField] float crashChargeDuration;
    [SerializeField] float crashChargeDelay;
    [SerializeField] float crashForce;

    [SerializeField] Animator thursterAnim;
    private int crashTrigger = Animator.StringToHash("Activation");


    // Important for Object pooling the bullets
    //private LaserBeam bullet;
    // private LaserBeam upgradedBullet1;
    // private LaserBeam upgradedBullet2;
    // ----------------------------------------

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // Ensures there is only one instance of ShipController
    }

    public void SetPlayerController(PlayerController controller)
    {
        _playerController = controller;

        _playerController.OnPrimaryFirePerformed += FireLaser;

        //_playerController.OnSecondaryFirePerformed += TetrisBlockManager.Instance.StartTetrisOverlay;
        //_playerController.OnSecondaryFireCancelled += TetrisBlockManager.Instance.TryFireTetrisCannon;
        _playerController.OnSecondaryFirePerformed += StartFireTag;
        _playerController.OnSecondaryFireCancelled += StopFireTag;
    }

    private void OnDestroy()
    {
        _playerController.OnPrimaryFirePerformed -= FireLaser;

        //_playerController.OnSecondaryFirePerformed -= TetrisBlockManager.Instance.StartTetrisOverlay;
        //_playerController.OnSecondaryFireCancelled -= TetrisBlockManager.Instance.TryFireTetrisCannon;
        _playerController.OnSecondaryFirePerformed -= StartFireTag;
        _playerController.OnSecondaryFireCancelled -= StopFireTag;
    }

    private void Start()
    {
        //AkSoundEngine.PostEvent("Play_SFX_Ship_Fly", gameObject);
        _spriteRenderer = GetComponent<SpriteRenderer>();

       /* weaponCooldowns.Add(WeaponType.LaserBeam, 0.2f);
        weaponCooldowns.Add(WeaponType.GravityBeam, 1.0f);
        weaponCooldowns.Add(WeaponType.Vortex, 1.5f);
        weaponCooldowns.Add(WeaponType.HomingMissle, 0.1f);
        weaponCooldowns.Add(WeaponType.ShotGun, 0.7f);
        weaponCooldowns.Add(WeaponType.MetalMine, 0.4f);

        timeSinceLastWeapon.Add(WeaponType.LaserBeam, 0.0f);
        timeSinceLastWeapon.Add(WeaponType.GravityBeam, 0.0f);
        timeSinceLastWeapon.Add(WeaponType.Vortex, 0.0f);
        timeSinceLastWeapon.Add(WeaponType.HomingMissle, 0.0f);
        timeSinceLastWeapon.Add(WeaponType.ShotGun, 0.0f);
        timeSinceLastWeapon.Add(WeaponType.MetalMine, 0.0f);*/

        //assign bullet
       /* _currentBullet = bulletCap;
        _secondBullet = bulletCap;*/

        //update weapon and bullet count
       // countBar.SetBulletCount((float)_currentBullet / bulletCap);
      //  countBar.SetWeaponName(_currentWeapon);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       /* if (collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("Hazard"))
        {
            GameManager.Instance.DamagePlayer(); // Call the DamagePlayer method from GameManager script

            Vector2 direction = (transform.position - collision.transform.position).normalized;
            _rb.AddForce(direction * _dashForce);

          //  AkSoundEngine.PostEvent("Play_VO_Pay_Attention", gameObject);
         //   AkSoundEngine.PostEvent("Play_SFX_Asteroid_Impact", gameObject);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeWeapon();
          //  weaponChangeVFX.body.position = transform.position;
          //  weaponChangeVFX.body.rotation = transform.rotation;
          //  weaponChangeVFX.SetOn();

        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canDash)
            {
                StartCoroutine(Dash());
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        movementDirection.Normalize();
        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(_shipThrust.transform.forward, -movementDirection);
            _shipThrust.transform.rotation = Quaternion.RotateTowards(_shipThrust.transform.rotation, Quaternion.Lerp(_shipThrust.transform.rotation, toRotation, 0.05f), 6);
        }
        _shipThrust.transform.position = transform.position;

        _timeSinceLastLaser += Time.deltaTime;
        _cameraTargetTransform.position = ((Vector2)transform.position + _playerController.MousePosition) / 2f;
    }

    public void ChangeWeapon()
    {
        //perform a swap between curernt and secondary
       /* WeaponType lastWeapon = _currentWeapon;
        _currentWeapon = _secondWeapon;
        _secondWeapon = lastWeapon;
        //swap bullet count
        int lastBullet = _currentBullet;
        _currentBullet = _secondBullet;
        _secondBullet = lastBullet;*/

        //update weapon and bullet count
       // countBar.SetBulletCount((float)_currentBullet / bulletCap);
       // countBar.SetWeaponName(_currentWeapon);
    }

    public void ChangeWeaponOnExpire()
    {
        //change to a weapon to right that is not secondary weapons or current weapons
      /*  WeaponType lastWeapon = _currentWeapon;
        for (int i = 0; i < 2; i++)//repeat twice to prevent the duplicate
        {
            _currentWeapon = (WeaponType)(((int)_currentWeapon + 1) % System.Enum.GetNames(typeof(WeaponType)).Length);
            //reset bullet
            _currentBullet = bulletCap;
            if (_currentWeapon != lastWeapon && _currentWeapon != _secondWeapon) break;

        }*/
        //update weapon and bullet count
      //  countBar.SetBulletCount((float)_currentBullet / bulletCap);
      //  countBar.SetWeaponName(_currentWeapon);

        //Crash change to enemies
        CrashCharge();
    }

    private void CrashCharge()
    {
        StartCoroutine(CrashChargeHandler());
      //  crashVFX.body.position = transform.position;
      //  crashVFX.SetOn();
        thursterAnim.SetTrigger(crashTrigger);
        //cam shake
      //  AkSoundEngine.PostEvent("Play_SFX_Asteroid_Impact", gameObject);
      //  CameraShake.instance.shakeCamera(7f, .3f);
    }
    private IEnumerator CrashChargeHandler()
    {
        Time.timeScale = .5f;
        yield return new WaitForSeconds(crashChargeDelay);
        Time.timeScale = 1;
        _rb.AddForce(transform.up * crashForce);
        chargeVFX.SetActive(true);
        yield return new WaitForSeconds(crashChargeDuration);
        EndCharging();
    }

    public void EndCharging()
    {
        //remove vfx
        chargeVFX.SetActive(false);
    }

    public IEnumerator Dash()
    {
        canDash = false;

        shipTail1.Clear();
        shipTail2.Clear();
        _shipThrust.gameObject.SetActive(false);
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(_dashEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(_dashDuration);

        //Get direction from playerpos to the camera
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = worldPos - new Vector2(transform.position.x, transform.position.y);
        //Insteaed of normalizing the direction, its being used to also check the distance, if its greater than max distance then setting it to max distance (limiting it) 
        if (direction.x > _maxDashDistance) direction.x = _maxDashDistance;
        else if (direction.x < -_maxDashDistance) direction.x = -_maxDashDistance;

        if (direction.y > _maxDashDistance) direction.y = _maxDashDistance;
        else if (direction.y < -_maxDashDistance) direction.y = -_maxDashDistance;
        //-------------------------

        transform.position = new Vector2(transform.position.x + direction.x, transform.position.y + direction.y);
        yield return new WaitForFixedUpdate();
       // CameraShake.instance.shakeCamera(7f, .2f);
        _shipThrust.gameObject.SetActive(true);
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(_dashCoolDown);
        canDash = true;
    }

    private float tagDuration = 0;

    private void FixedUpdate()
    {
     /*   if (_isFiringTag &&
            GameManager.Instance.PlayerInventory.CheckResource(ResourceType.SHRAPNEL) >= 1)
        {
            tagDuration += Time.deltaTime;
            if (tagDuration > 0.2f)
            {
                GameManager.Instance.PlayerInventory.ChangeResource(ResourceType.SHRAPNEL, -1);
                tagDuration = 0f;
            }
            FireTag();
        }
        else
        {
            StopFireTag();
        }*/


        HandleShipMovement();
        HandleShipRotation();
    }

    private void FireLaser()
    {
     /*   if (Time.time - timeSinceLastWeapon[_currentWeapon] >= weaponCooldowns[_currentWeapon])
        {

            // --Fire With Object Pool--
            IProjectile projectile = ObjectPool.instance.GetProjectile(_currentWeapon);
            if (projectile != null)
            {
                projectile.GetBody().position = _firePoint.position;
                projectile.GetBody().rotation = _firePoint.rotation;
                projectile.SetFire();
                AkSoundEngine.PostEvent("Play_SFX_Ship_Blaster", gameObject);
            }

            if (upgradedBullets)
            {
                AkSoundEngine.SetState("Weapon_Upgrade", "Upgraded");
                // --Fire With Object Pool--
                IProjectile projectile1 = ObjectPool.instance.GetProjectile(_currentWeapon);
                if (projectile1 != null)
                {
                    projectile1.GetBody().position = _firePoint.position;
                    projectile1.GetBody().rotation = _firePoint.rotation * Quaternion.Euler(0, 0, 15);
                    projectile1.SetFire();
                }
                IProjectile projectile2 = ObjectPool.instance.GetProjectile(_currentWeapon);
                if (projectile2 != null)
                {
                    projectile2.GetBody().position = _firePoint.position;
                    projectile2.GetBody().rotation = _firePoint.rotation * Quaternion.Euler(0, 0, -15);
                    projectile2.SetFire();
                }
            }

            //this is a fire count bbullet down
            _currentBullet--;
            countBar.SetBulletCount((float)_currentBullet / bulletCap);
            if (_currentBullet <= 0) ChangeWeaponOnExpire();

            var muzzle = Instantiate(_muzzleFlashPrefab, _firePoint.position, _firePoint.rotation, _firePoint);

            Destroy(muzzle, .1f);
            timeSinceLastWeapon[_currentWeapon] = Time.time;
        }
*/

    }

    private void StartFireTag()
    {
        /*if (_canLaserTag && !_isFiringTag)
        {
            _isFiringTag = true;
        }*/
    }

    private void StopFireTag()
    {
        /*_isFiringTag = false;
        if (_currentTag != null)
        {
            Destroy(_currentTag);
        }*/
    }

    private void FireTag()
    {
        /*Vector2 rayStartPoint = _firePoint.position;
        Vector2 rayDirection = _firePoint.up.normalized; // Use _firePoint.up to get the laser direction

        // Cast a ray and get all hits (including the laser itself)
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayStartPoint, rayDirection, 10f);

        float nearestHitDistance = 10f;

        // Iterate through all the hits
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Asteroid"))
            {
                Vector2 hitPoint = hit.point;
                float tagDistance = Vector2.Distance(rayStartPoint, hitPoint);
                nearestHitDistance = Mathf.Min(nearestHitDistance, tagDistance);
            }
        }
        if (_currentTag == null)
            _currentTag = Instantiate(_tagBeamPrefab, _firePoint.position, _firePoint.rotation);
        _currentTag.transform.position = rayStartPoint + rayDirection * nearestHitDistance / 2f;
        _currentTag.transform.rotation = Quaternion.FromToRotation(Vector2.up, rayDirection);
        _currentTag.transform.localScale = new Vector3(2f, nearestHitDistance, 1f);

        var muzzle = Instantiate(_muzzleTagFlashPrefab, _firePoint.position, _firePoint.rotation, _firePoint);

        Destroy(muzzle, .1f);*/
    }


    private void HandleShipMovement()
    {
        Vector2 force = new(_playerController.MoveInput.x * Time.fixedDeltaTime, _playerController.MoveInput.y * Time.fixedDeltaTime);
        force.Normalize();

        _rb.AddForce(force * _currentSpeed);
        // Calculate the current speed of the ship
        float speed = _rb.velocity.magnitude;

        // Set the RTPC value in Wwise using AkSoundEngine.SetRTPCValue
       // AkSoundEngine.SetRTPCValue("Ship_Speed", speed);

        // Set the RTPC value in Wwise using AkSoundEngine.SetRTPCValue
        //AkSoundEngine.SetRTPCValue("Ship_Speed", speed);
    }


    private void HandleShipRotation()
    {
        Vector2 lookDirection = _playerController.MousePosition - _rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

        _rb.rotation = angle;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("Shrapnel"))
        {
           // PlayerResourceInventory.instance.ChangeResource(ResourceType.SHRAPNEL, 1);
            Destroy(collision.gameObject);
        }*/
    }

}
