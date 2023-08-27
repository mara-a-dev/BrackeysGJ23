using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource CoinSound;
    public AudioSource TrashSound;
    public AudioSource TreasureSound;

    private void Awake()
    {
        Player.OnItemCollected += PlaySound;
    }

    private void PlaySound(Item.Types item)
    {
        if (item == Item.Types.coin)
        {
            CoinSound.Play();
        }
        if (item == Item.Types.trash)
        {
            TrashSound.Play();
        }
        if (item == Item.Types.treasure)
        {
            TreasureSound.Play();
        }

    }

    private void OnDestroy()
    {
        Player.OnItemCollected -= PlaySound;
    }
}
