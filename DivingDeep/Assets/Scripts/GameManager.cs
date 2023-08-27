using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI TrashText;
    public TextMeshProUGUI TreasureText;

    public Transform GameOverScreen;
    public Transform WinningScreen;

    private int coin = 0;
    private int trash = 0;
    private int treasure = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        GameOverScreen.gameObject.SetActive(false);
        WinningScreen.gameObject.SetActive(false);
        Player.OnItemCollected += CollectedItem;
    }


    private void CollectedItem(Item.Types item)
    {
        if (item == Item.Types.coin)
        {
            coin++;
            CoinText.text = coin.ToString();
        }
        if (item == Item.Types.trash)
        {
            trash++;
            TrashText.text = trash.ToString();

            if (trash == 25)
                YouWon();
        }
        if (item == Item.Types.treasure)
        {
            treasure++;
            TreasureText.text = treasure.ToString();
        }
    }

    public void GameOver()
    {
        GameOverScreen.gameObject.SetActive(true);
        Debug.Log("-----GAME OVER-----");
    }
    public void YouWon()
    {
        WinningScreen.gameObject.SetActive(true);
        Debug.Log("-----You Won-----");
    }

    private void OnDestroy()
    {
        Player.OnItemCollected -= CollectedItem;
    }

}
