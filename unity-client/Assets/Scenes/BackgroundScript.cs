using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private Material mat;
    private float fireworkTimer = -1f; 
    private float spawnCooldown = 10f; 

    private void Start()
    {
        
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
     
        spawnCooldown -= Time.deltaTime;

        if (spawnCooldown <= 0)
        {
          
            fireworkTimer = 1.0f;

      
            spawnCooldown = Random.Range(5f,20f);
        }

        if (fireworkTimer > 0)
        { 
            mat.SetFloat("_FireworkValue", fireworkTimer);

            fireworkTimer -= Time.deltaTime;
        }
        else if (fireworkTimer <= 0 && fireworkTimer > -1f)
        {

            mat.SetFloat("_FireworkValue", 0);
            fireworkTimer = -1f;
        }
    }
}
