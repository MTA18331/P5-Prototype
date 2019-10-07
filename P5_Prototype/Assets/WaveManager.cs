using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    int waitTime;
    public static WaveManager instance = null;
    public float timeToReachTarget;
    public Texture2D wavePic;
    public GameObject Projectile;



    void Awake()
    {
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);


        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        buildWave();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void buildWave()
    {
        
        var whitePixels = 0;
        var blackPixels = 0;
        for (int i = 0; i < wavePic.width; i++)
            for (int j = 0; j < wavePic.height; j++)
            {
                Color pixel = wavePic.GetPixel(i, j);

                
                if (pixel == Color.black)
                {
                    spawnProjectile(i, j);
                    blackPixels++;
                }
                else
                {
                    whitePixels++;
                }
                   
            }
        Debug.Log(string.Format("White pixels {0}, black pixels {1}", whitePixels, blackPixels));
    }

    void spawnProjectile(int x, int y)
    {
        GameObject pro = null;
        // spawn a top, mid or bot projectile according to y and make it wait according to x
        pro = Instantiate(Projectile, transform.position, Quaternion.identity);
        pro.GetComponent<projectile>().waitTime = x;
        switch (y)
        {
            case 0:
                pro.GetComponent<projectile>().setEnum("bot");
                break;
            case 1:
                pro.GetComponent<projectile>().setEnum("mid");
                break;
            case 2:
                pro.GetComponent<projectile>().setEnum("top");
                break;

            default:
                Debug.Log("Well, shit");
               
                break;
        }
       
    }
}
