using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{


    public ShipMode CurrentShipMode => _currentShipMode;
    private ShipMode _currentShipMode;
    private Camera _camera;
    private PlayerInput _playerInputController;

    [SerializeField] public GroundedController _groundedController;
    public ShipController _shipController;

    public Vector2 MoveInput { get; private set; }
    public Vector2 MousePosition { get; private set; }

    public event Action OnPrimaryFirePerformed;

    public event Action OnSecondaryFirePerformed;
    public event Action OnSecondaryFireCancelled;
    

    private void Awake()
    {
        _camera = Camera.main;
        _playerInputController = GetComponent<PlayerInput>();

        _groundedController.SetPlayerController(this);
        _shipController.SetPlayerController(this);

    }
    public void Start()
    {
        _groundedController.IsFiringEnabled = false;

    }
    private void Update()
    {
        MousePosition = _camera.ScreenToWorldPoint(_playerInputController.actions.FindAction("Aim").ReadValue<Vector2>());

        // Enter Upgrade State
        // if(GameManager.Instance.CurrentState == GameState.GROUNDED && Input.GetKeyDown(KeyCode.E))
        //     GameManager.Instance.ChangeGameState(GameState.UPGRADING);
        // Exit Upgrade State
   /*     if(GameManager.Instance.CurrentState == GameState.UPGRADING && Input.GetKeyDown(KeyCode.Escape))
            ExitUpgradeState();*/
    }

    public void ExitUpgradeState()
    {
       /* if(UpgradeMenuView.Instance.isPlacingTurrets) return;
        if (UpgradeMenuView.Instance.isSellingTurrets)
            UpgradeMenuView.Instance.exitTurretSellMenu();
        GameManager.Instance.ChangeGameState(GameState.EXPLORING);*/       
    }

    public void SwitchShipMode(ShipMode newMode)
    {
        if (_currentShipMode == newMode) return;
        _currentShipMode = newMode;

        switch (newMode)
        {
            case ShipMode.SHUTTLE:
                _shipController.transform.position = new Vector2(_groundedController.transform.position.x,
                                                                     _shipController.transform.position.y);
                _shipController.gameObject.SetActive(true);
                _groundedController.gameObject.SetActive(false);


               // _shipController.IsFiringEnabled = true;
                _groundedController.IsFiringEnabled = false;
               // AkSoundEngine.PostEvent("Play_SFX_Ship_Takeoff", gameObject);
                break;
            // case ShipMode.GROUNDED:
            //     _shipController.gameObject.SetActive(false);
            //     _groundedController.transform.position = new Vector2(_shipController.transform.position.x,
            //                                                          _groundedController.transform.position.y);
            //     _groundedController.gameObject.SetActive(true);

            //     _shipController.IsFiringEnabled = false;
            //     _groundedController.IsFiringEnabled = true;
            //     AkSoundEngine.PostEvent("Play_SFX_Ship_Land", gameObject);
            //     break;
            case ShipMode.NONE:
                _shipController.gameObject.SetActive(false);
                _groundedController.gameObject.SetActive(false);
              //  AkSoundEngine.SetState("Ship_Flying", "None");

                break;
        }
    }

    // Input Methods

    public void SetMoveInput(CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    public void TriggerPrimaryFire(CallbackContext ctx)
    {
        //check on UI

        if(ctx.performed)
        {
            OnPrimaryFirePerformed?.Invoke();
        }
    }

    public void TriggerSecondaryFire(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnSecondaryFirePerformed?.Invoke();
        } else if(ctx.canceled)
        {
            OnSecondaryFireCancelled?.Invoke();
        }
    }

    public void ToggleControllerMode(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        switch(GameManager.Instance.CurrentState)
        {
            // case GameState.GROUNDED:
            //     SwitchShipMode(ShipMode.SHUTTLE);
            //     GameManager.Instance.ChangeGameState(GameState.EXPLORING);
            //     break;
            case GameState.EXPLORING:
                if (!GameManager.Instance.DockingAllowed) return;
                SwitchShipMode(ShipMode.UPGRADING);
                GameManager.Instance.ChangeGameState(GameState.UPGRADING);
                break;
            case GameState.DEFENDING:
                // Toggle our play style without changing game state
                SwitchShipMode(ShipMode.SHUTTLE);
                break;
        }
    }
}


public enum ShipMode
{
    SHUTTLE,
    UPGRADING,
    // GROUNDED,
    NONE
}
