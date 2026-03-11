using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject plane;
    public GameObject obstacle;
    public GameObject background;

    public float spawnFrequency= 2f;
    float lastSpawn = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(plane, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(background, new Vector3(0, 0, 3), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        lastSpawn -= Time.deltaTime;

        if (lastSpawn < 0)
        {
            spawnObstacle();
            lastSpawn = spawnFrequency;
        }
    }

    void spawnObstacle()
    {
        float pos = Random.Range(-5f, 5f);
        Instantiate(obstacle, new Vector3(12, pos, 0), Quaternion.identity);
    }
}
