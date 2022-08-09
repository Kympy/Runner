using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isJumpedSecond = false;
    private bool isGrounded = true;
    [SerializeField]
    private float jumpPower = 0f;
    public float JumpPower { get { return jumpPower; } }
    private float maxJumpPower = 400f;
    private float rayDistance = 0.5f;
    private RaycastHit hit;
    private Rigidbody playerRigidbody;
    private Animator anim;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.red);
        if(Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.transform.tag == "Tile")
            {
                isGrounded = true;
                isJumpedSecond = false;
                anim.SetBool("IsJump", false);
            }
        }
        else
        {
            anim.SetBool("IsJump", true);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isJumpedSecond == false && isGrounded == false) // Double Jump
        {
            playerRigidbody.AddForce(Vector3.up * maxJumpPower / 2, ForceMode.Impulse);
            isJumpedSecond = true;
            jumpPower = 0f;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded) // Key Pushing
        {
            jumpPower += Time.deltaTime * 1000f;
        }
        if(Input.GetKeyUp(KeyCode.Space) && isGrounded) // Jump
        {
            if (isGrounded && isJumpedSecond == false)
            {
                //anim.SetBool("IsJump", true); 
                if (jumpPower > maxJumpPower)
                {
                    jumpPower = maxJumpPower;
                }
                playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                isGrounded = false;
            }
            jumpPower = 0f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            GameObject.FindObjectOfType<GameManager>().GameOver();
        }
    }
}
