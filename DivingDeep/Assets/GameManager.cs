using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private AsteroidGrid _asteroidGrid;
    public AsteroidGrid AsteroidGrid => _asteroidGrid;

    public PlayerController _playerController;

    public SpriteRenderer _shipSprite;
    public bool DockingAllowed { get; private set; }
    public bool onUI;

    public Vector2 RespawnPos;

    public int MaxCityHealth;
    public int MaxPlayerHealth;
    public int CurrentCityHealth { get; private set; }
    public int CurrentPlayerHealth { get; private set; }

    [SerializeField] private GameState _initialGameState = GameState.SETUP;
    private GameState _currentState;
    public GameState CurrentState => _currentState;

    public event Action<float> OnCityHealthChangePercentage;
    public event Action<float> OnPlayerHealthChangePercentage;
    [SerializeField] private int StartShrapnel;

    // Camera Controls
    [SerializeField] public Camera _mainCamera;
    [SerializeField] public GameObject _cityCamera;
    [SerializeField] public GameObject _defenceCamera;
    [SerializeField] public GameObject _exploreCamera;
    [SerializeField] public GameObject _turretPlaceCamera;
    //public PlayerResourceInventory PlayerInventory { get; private set; }
    public bool gameEnded { get; private set; }

   // public TurretList turretList;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

     //   PlayerInventory = new();
    }

    void Start()
    {
        ChangeGameState(_initialGameState);
       // PlayerInventory.ChangeResource(ResourceType.SHRAPNEL, StartShrapnel);
    }

    public void ChangeGameState(GameState newGameState)
    {
        _currentState = newGameState;
        switch (_currentState)
        {
            case GameState.SETUP:
                HandleGameSetup();
                break;
            case GameState.EXPLORING:
                HandlePlayerExploring();
                break;
            case GameState.GROUNDED:
                HandlePlayerGrounded();
                break;
            case GameState.DEFENDING:
                HandlePlayerDefending();
                break;
            case GameState.UPGRADING:
                HandlePlayerUpgrading();
                break;
            case GameState.PLAYER_WIN:
                HandlePlayerWin();
                break;
            case GameState.PLAYER_LOSE:
                HandlePlayerLose();
                break;
        }
    }

    private void HandleGameSetup()
    {
        CurrentCityHealth = MaxCityHealth;
        CurrentPlayerHealth = MaxPlayerHealth;
        OnCityHealthChangePercentage?.Invoke(CurrentCityHealth);

        ChangeGameState(GameState.EXPLORING);
    }

    private void HandlePlayerExploring()
    {
        _exploreCamera.SetActive(true);
        _defenceCamera.SetActive(false);
        _cityCamera.SetActive(false);

       // UIViewManager.Show<ExplorationHUDView>(false);
        ExitSlowDown();
    }

    private void HandlePlayerGrounded()
    {

        /*if (AlienManager.Instance.HasEggsToHatch)
        {
            // Hatch all eggs that we have
            AlienManager.Instance.HatchActiveEggs();

            *//*if(AlienManager.Instance.HasAggressiveAliensSpawned)
            {
                // Immediately enter battle
                ChangeGameState(GameState.DEFENDING);
                return;
            }*//*
        }*/

        _exploreCamera.SetActive(false);
        _defenceCamera.SetActive(false);
        _turretPlaceCamera.SetActive(false);
        _cityCamera.SetActive(true);

        //UIViewManager.Show<CityHUDView>(false);
    }

    private void Update()
    {
        /*if (!gameEnded &&
            !_asteroidGrid.HasAstroids &&
            !AlienManager.Instance.HasAlienOrEgg)
        {
            ChangeGameState(GameState.PLAYER_WIN);
        }*/
       // AkSoundEngine.SetRTPCValue("CityHealth", CurrentCityHealth);
    }

    private void HandlePlayerDefending()
    {
        _exploreCamera.SetActive(false);
        _defenceCamera.SetActive(true);
        _turretPlaceCamera.SetActive(false);
        _cityCamera.SetActive(false);

      //  UIViewManager.Show<BattleHUDView>(false);
    }

    private void HandlePlayerUpgrading()
    {
      //  UIViewManager.Show<UpgradeMenuView>(false);
        _turretPlaceCamera.SetActive(false);
        _exploreCamera.SetActive(false);
        _cityCamera.SetActive(true);
        DoSlowDown();
    }

    private void HandlePlayerWin()
    {
        gameEnded = true;
      /*  UIViewManager.Show<WinView>();
        AkSoundEngine.SetState("Game_Condition", "Win_Screen");
        AkSoundEngine.PostEvent("Play_Game_Win", gameObject);*/

    }

    private void HandlePlayerLose()
    {
        gameEnded = true;
     /*   UIViewManager.Show<GameOverView>();
        AkSoundEngine.SetState("Game_Condition", "Lose_Screen");
        AkSoundEngine.PostEvent("Play_Game_Lose", gameObject);*/
    }

    public void DamageCity()
    {
        CurrentCityHealth -= 4;
        //Debug.Log(CurrentCityHealth);
        OnCityHealthChangePercentage?.Invoke(CurrentCityHealth);
        if (CurrentCityHealth <= 0 && CurrentState != GameState.PLAYER_LOSE)
        {
            ChangeGameState(GameState.PLAYER_LOSE);
         //   AkSoundEngine.PostEvent("Stop_SFX_Ship_Fly", gameObject);
        }
    }

    public void DamagePlayer()
    {
        StartCoroutine(ShakeCam(0.25f, 0.001f));
        CurrentPlayerHealth -= 12;
        if (CurrentPlayerHealth >= 0)
            OnPlayerHealthChangePercentage?.Invoke(CurrentPlayerHealth);

        if (CurrentPlayerHealth <= 0 && CurrentState != GameState.PLAYER_LOSE)
        {
            ChangeGameState(GameState.PLAYER_LOSE);
          //  AkSoundEngine.PostEvent("Stop_SFX_Ship_Fly", gameObject);
        }
    }
    public void UpdatePlayerHealth(int newHealth)
    {
        CurrentPlayerHealth = newHealth;
        if (CurrentPlayerHealth >= 0)
            OnPlayerHealthChangePercentage?.Invoke(CurrentPlayerHealth);
    }

    public void HandleBattleCompleted()
    {
        switch (_playerController.CurrentShipMode)
        {
            case ShipMode.SHUTTLE:
                ChangeGameState(GameState.EXPLORING);
                break;
                // case ShipMode.GROUNDED:
                //     ChangeGameState(GameState.GROUNDED);
                //     break;
        }
    }

    public IEnumerator ShakeCam(float duration, float magnitude)
    {
        Vector3 originalPos = _exploreCamera.transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float xOffset = UnityEngine.Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = 0.5f * magnitude;
            _exploreCamera.transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z);
            elapsedTime += Time.deltaTime;
            _shipSprite.color = Color.red;
            yield return null;
        }
        _exploreCamera.transform.localPosition = originalPos;
        _shipSprite.color = Color.white;
    }

    public void AllowDocking(bool allowDocking)
    {
        DockingAllowed = allowDocking;
    }

    public void RestartGame()
    {
       /* SceneChangeManager.Instance.OpenGameScene();
        AkSoundEngine.SetState("Game_Condition", "Gameplay");
        AkSoundEngine.PostEvent("Stop_Game_Lose", gameObject);
        AkSoundEngine.PostEvent("Stop_Game_Win", gameObject);*/
    }

    public void QuitToMenu()
    {
       /* SceneChangeManager.Instance.OpenMainMenuScene();
        AkSoundEngine.SetState("Game_Condition", "Menu");
        AkSoundEngine.PostEvent("Stop_SFX_Ship_Fly", gameObject);
        AkSoundEngine.PostEvent("Stop_Game_Lose", gameObject);
        AkSoundEngine.PostEvent("Stop_Game_Win", gameObject);*/
    }

    public void DoSlowDown()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0.25f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }

    public void ExitSlowDown()
    {
        if (Time.timeScale == 0.25f)
        {
            Time.timeScale = 1;
        }
    }

}


public enum GameState
{
    SETUP,
    EXPLORING,
    GROUNDED,
    DEFENDING,
    UPGRADING,
    PLAYER_WIN,
    PLAYER_LOSE
}
