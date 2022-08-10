using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float GameSpeed;
    public float GetGameSpeed { get { return GameSpeed; } }
    private int score = -100;
    public int Score {  get { return score; } }
    private CanvasGroup myCanvas;
    private TextMeshProUGUI myScore;
    private GameObject tileObj = null;
    private GameObject nextTile = null;
    private float tileScale;
    private PlayerMovement player;
    private Slider fillImg;
    private bool isGameOver = false;

    private void Awake()
    {
        tileObj = Resources.Load("Tile") as GameObject;
        myCanvas = GameObject.FindObjectOfType<CanvasGroup>();
        myScore = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        fillImg = GameObject.FindObjectOfType<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        ChangeGameSpeed(4f);
    }
    private void Start()
    {
        myCanvas.alpha = 0f;
        fillImg.value = player.JumpPower;
    }
    private void Update()
    {
        fillImg.value = player.JumpPower; // Update Jump power UI
        myScore.text = "SCORE : " + score.ToString();
        if (Input.GetKeyDown(KeyCode.R) && isGameOver) // Restart Game
        {
            Time.timeScale = 1f;
            myCanvas.alpha = 0f;
            isGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Get current Scene's build index and reload scene
        }
    }
    public void CreateNextTile(Vector3 previousPos) // Create Tile
    {
        tileScale = Random.Range(4f, 10f); // Set Tile Scalse
        previousPos += new Vector3(0f, 0f, Random.Range(5f, 9f)); // Previous position + new Random Position
        previousPos.y = Random.Range(-2f, 1f); // new Random Y position (up down)
        nextTile = Instantiate(tileObj, previousPos, Quaternion.identity); // Create
        nextTile.transform.localScale = new Vector3(5f, 1f, tileScale); // Set Scale
    }
    private void ChangeGameSpeed(float speed)
    {
        GameSpeed = speed;
    }
    public void GameOver()
    {
        Time.timeScale = 0f; // Stop Game
        myCanvas.alpha = 1f;
        isGameOver = true;
    }
    public void GetScore()
    {
        score += 100;
    }
}
