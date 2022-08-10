using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour // Tile Object Script
{
    private float tileSpeed; // tileSpeed
    private Vector3 screenPoint; // Screen Point
    private GameObject obstacle; // My Obstacle
    private Transform endOfTile; // End Of Tile
    private bool willDestroy = false; // If the tile will destroy?
    private bool firstTouch = true; // Player touch this tile
    private GameManager GM = null; // Find GM
    private int obsCount = 0; // Count of my obstacle
    private void Awake()
    {
        GM = GameObject.FindObjectOfType<GameManager>(); // Find
        endOfTile = transform.GetChild(0); // Find My End Tip
    }
    private void Start()
    {
        tileSpeed = GM.GetGameSpeed; // Get
        obstacle = Resources.Load("Obstacle") as GameObject; // Get
        if(Random.Range(0, 100) < 10) // Obstacle created by 10% random
        {
            obsCount = Random.Range(1, 4); // Count of obstacle
            for (int i = 0; i < obsCount; i++)
            {
                GameObject empty = new GameObject();
                empty.transform.SetParent(transform); // Create new Empty object for fixing scaling of child object
                empty.transform.localPosition = Vector3.zero;
                empty.transform.localPosition += new Vector3(0f, 0f, Random.Range(-0.35f, 0.35f)); // Random Position
                GameObject obj = Instantiate(obstacle, empty.transform); // Create Obstacle
                obj.transform.SetParent(empty.transform);
            }
        }
    }
    private void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * tileSpeed); // Tile Movement
        screenPoint = Camera.main.WorldToViewportPoint(endOfTile.position); // Screen Point Of End Of Tile

        if (screenPoint.z > 0 &&
            screenPoint.x > 0 && screenPoint.x < 1 &&
            screenPoint.y > 0 && screenPoint.y < 1) // In Camera
        {
            //Debug.Log("tile In");
            if(willDestroy == false) // This tile will destroy!
            {
                GM.CreateNextTile(endOfTile.position); // So Create Next One!
                willDestroy = true;
            }
        }
        else // Out of Camera
        {
            if(willDestroy) // Can I destroy this tile?
            {
                //Debug.Log("Tile out");
                Destroy(this.gameObject); // Yes,  you can
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player") // Collision object name
        {
            if(collision.gameObject.transform.position.y > transform.position.y) // Is player above on tile?
            {
                if(firstTouch) // Player's first touch
                {
                    GM.GetScore(); // Then, Get score!
                    firstTouch = false;
                }
            }
        }
    }
}
