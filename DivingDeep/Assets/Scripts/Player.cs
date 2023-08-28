using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static event Action<Item.Types> OnItemCollected;
    public static event Action OnTimeEnding;

    public GameObject Bubbles;
    public GameObject JumpText;

    public Slider BreathingSlider;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 15.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -50f;
    private Animator anim;
    private Vector3 lastPos;
    private bool isUnderWater = false;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        anim = GetComponent<Animator>();
        // this.GetComponent<Rigidbody>().freezeRotation = true;
    }

    void Update()
    {

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;

        }


        if (transform.position != lastPos && !isUnderWater)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }

        if (transform.position != lastPos && isUnderWater)
        {
            anim.SetBool("swim", true);
        }
        else
        {
            anim.SetBool("swim", false);
        }

        lastPos = transform.position;

        if (Input.GetKeyDown("space"))
        {
            anim.SetBool("walk", false);
            anim.SetBool("dive", true);
            if (!isUnderWater)
                Invoke("SetUnderwater",0.25f);
        }

        /*// Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }*/

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        // transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Jump"))
        {
            JumpText.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            OnItemCollected?.Invoke(Item.Types.coin);
        }
        if (collision.gameObject.CompareTag("Trash"))
        {

            Destroy(collision.gameObject);
            OnItemCollected?.Invoke(Item.Types.trash);
        }
        if (collision.gameObject.CompareTag("Treasure"))
        {
            Destroy(collision.gameObject);
            OnItemCollected?.Invoke(Item.Types.treasure);
        }
        if (collision.gameObject.CompareTag("Bubble"))
        {
            OnItemCollected?.Invoke(Item.Types.bubble);
            ResetBreathing();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Jump"))
        {
            JumpText.SetActive(false);
        }
    }

    public void SetUnderwater()
    {
        isUnderWater = true;
        gravityValue = 0;
        StartCoroutine(LerpBreathing());
        Bubbles.SetActive(true);
    }

    private void ResetBreathing()
    {
        StopAllCoroutines();
        StartCoroutine(LerpBreathing());
    }

    private IEnumerator LerpBreathing()
    {
        float progress = 0;
        float duration = 0.009f;
        float startValue = BreathingSlider.maxValue;
        float endValue = BreathingSlider.minValue;

        BreathingSlider.value = startValue;

        while (BreathingSlider.value > endValue)
        {
            BreathingSlider.value -= duration * Time.deltaTime;

            if (BreathingSlider.value <= BreathingSlider.minValue)
            {
                OnTimeEnding?.Invoke();
                GameManager.Instance.GameOver();
                yield break;
            }
            yield return null;
        }
        BreathingSlider.value = endValue;
        yield break;
    }
}
