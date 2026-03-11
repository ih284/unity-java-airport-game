using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float obstacleSpeed = 0.005f;
    float y;
    float x;
    public Sprite ObstacleSprite1;
    public Sprite ObstacleSprite2;
    public Sprite ObstacleSprite3;
    int rnum;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        x = transform.position.x;
        y = transform.position.y;
        rnum = Random.Range(1, 2);
        if (rnum == 1)
        {
            spriteRenderer.sprite = ObstacleSprite1;
        }
        else
        {
            spriteRenderer.sprite = ObstacleSprite2;
        }
    }
        // Update is called once per frame
    void Update()
        {
            x = transform.position.x;
            transform.position = new Vector3(x - obstacleSpeed * Time.deltaTime, y, 0);
            if (x < -12)
            {
                Destroy(gameObject);
            }
        }

    }

