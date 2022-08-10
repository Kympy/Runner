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
        //Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.red);
        if(Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance)) // Check Ground
        {
            if (hit.transform.tag == "Tile") // If Tile~
            {
                isGrounded = true; // Player is on the ground
                isJumpedSecond = false; // Double jump not yet
                anim.SetBool("IsJump", false); // Play animation
            }
        }
        else // Player is not on the ground >> Player is jumping now
        {
            anim.SetBool("IsJump", true); // Anim play
        }
        if (Input.GetKeyDown(KeyCode.Space) && isJumpedSecond == false && isGrounded == false) // Double Jump
        {
            playerRigidbody.AddForce(Vector3.up * maxJumpPower / 2, ForceMode.Impulse);
            isJumpedSecond = true;
            jumpPower = 0f;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded) // Key Pushing
        {
            jumpPower += Time.deltaTime * 1000f; // Add jump power
        }
        if(Input.GetKeyUp(KeyCode.Space) && isGrounded) // Jump
        {
            if (isGrounded && isJumpedSecond == false) // First Jump
            {
                if (jumpPower > maxJumpPower) // If jumpPower is too high,
                {
                    jumpPower = maxJumpPower; // Then restrict jump power
                }
                playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); // Real Jump
                isGrounded = false; // Player is jumping!!
            }
            jumpPower = 0f; // If jump end, reset jump power;
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
