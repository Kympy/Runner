using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private float tileSpeed;
    private Vector3 screenPoint;
    private GameObject obstacle;
    private Transform endOfTile;
    private bool willDestroy = false;
    private bool firstTouch = true;
    private GameManager GM = null;
    private int obsCount = 0;
    private void Awake()
    {
        GM = GameObject.FindObjectOfType<GameManager>();
        endOfTile = transform.GetChild(0);
    }
    private void Start()
    {
        tileSpeed = GM.GetGameSpeed;
        obstacle = Resources.Load("Obstacle") as GameObject;
        if(Random.Range(0, 100) < 10)
        {
            obsCount = Random.Range(1, 4);
            for (int i = 0; i < obsCount; i++)
            {
                GameObject empty = new GameObject();
                empty.transform.SetParent(transform);
                empty.transform.localPosition = Vector3.zero;
                empty.transform.localPosition += new Vector3(0f, 0f, Random.Range(-0.35f, 0.35f));
                GameObject obj = Instantiate(obstacle, empty.transform);
                obj.transform.SetParent(empty.transform);
            }
        }
    }
    private void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * tileSpeed);
        screenPoint = Camera.main.WorldToViewportPoint(endOfTile.position);

        if (screenPoint.z > 0 &&
            screenPoint.x > 0 && screenPoint.x < 1 &&
            screenPoint.y > 0 && screenPoint.y < 1) // 시야에 들어오면
        {
            //Debug.Log("tile In");
            if(willDestroy == false)
            {
                GM.CreateNextTile(endOfTile.position);
                willDestroy = true;
            }
        }
        else // 시야를 벗어나면
        {
            if(willDestroy)
            {
                //Debug.Log("Tile out");
                Destroy(this.gameObject);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.transform.position.y > transform.position.y)
            {
                if(firstTouch)
                {
                    GM.GetScore();
                    firstTouch = false;
                }
            }
        }
    }
}
