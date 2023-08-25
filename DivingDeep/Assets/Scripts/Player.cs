using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 15.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9f;
    private Animator anim;
    private Vector3 lastPos;
    private bool isUnderWater = false;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        anim = GetComponent<Animator>();
        this.GetComponent<Rigidbody>().freezeRotation = true;
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
            isUnderWater = true;
            gravityValue = 0;
        }

        /*// Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }*/

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
        }
    }
}
