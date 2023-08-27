using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource CoinSound;
    public AudioSource TrashSound;
    public AudioSource TreasureSound;

    public AudioSource BubbleSound;
    public AudioSource ClockSound;

    private void Awake()
    {
        Player.OnItemCollected += PlaySound;
        Player.OnTimeEnding += PlayClockSound;
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
        if (item == Item.Types.bubble)
        {
            BubbleSound.Play();
        }

    }

    private void PlayClockSound()
    {
        ClockSound.Play();
    }



    private void OnDestroy()
    {
        Player.OnItemCollected -= PlaySound;
        Player.OnTimeEnding -= PlayClockSound;
    }
}
