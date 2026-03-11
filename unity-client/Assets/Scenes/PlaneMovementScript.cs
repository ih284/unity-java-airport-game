using JetBrains.Annotations;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class PlaneMovementScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject endScreen;
    public static event Action PlaneDied;
    public static event Action ScoreChanged;
    public Sprite StartSprite;
    public Sprite DeadSprite;
    public static GameManager Instance;
    public float gravity = 20.0f; 
    public float thrust = 500.0f;  
    public float xPos = -7;
    int score = 0;
    float momentum;
    bool dead = false;
    float scoretimer;
    float loadLeaderboardtime = 3;
    bool loadLeaderboard = false;
    float scoretime = 2;
    string name;
    private void Start()
    {
        name = PlayerPrefs.GetString("userName","NotSet");
        Debug.Log(name);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = StartSprite;
        
    }
    void Update()
    {
       
        scoretimer -= Time.deltaTime;
        if (scoretimer < 0)
        {
            score = score + 1;
            scoretimer = scoretime;
            ScoreChanged?.Invoke();
        }
        float y = transform.position.y;

        if (y < -5 || y > 5)
        {
            loadLeaderboard = true;
            PlaneDied?.Invoke();
            
            GameObject managerObj = GameObject.Find("GameManager");

            if (managerObj != null){         
            LeaderboardHandler lb = managerObj.GetComponent<LeaderboardHandler>();

            if (lb != null)
            {
            lb.postScore();
            }
            else
            {
            Debug.LogError("The object 'GameManager' does not have a LeaderboardHandler script!");
            }
            }
   
            Instantiate(endScreen, new Vector3(0, 0, -0.1f), Quaternion.identity);
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.Mouse0) && !dead)
        {
            momentum += thrust * Time.deltaTime;
        }
        momentum -= gravity * Time.deltaTime;

        transform.position = new Vector3(xPos,y+ momentum * Time.deltaTime, 0);      
        float rotationTarget = Mathf.Clamp(momentum * 5f, -30f, 30f);
        transform.rotation = Quaternion.Euler(0, 0, rotationTarget);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        spriteRenderer.sprite = DeadSprite;
        dead = true;
    }
}