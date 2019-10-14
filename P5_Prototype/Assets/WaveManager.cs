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
        var coloredPixels = 0;
        for (int i = 0; i < wavePic.width; i++)
            for (int j = 0; j < wavePic.height; j++)
            {
                Color pixel = wavePic.GetPixel(i, j);

                
                if (pixel == Color.red)
                {
                    spawnProjectile(i, "top");
                    coloredPixels++;
                }

                else if (pixel == Color.green)
                {
                    spawnProjectile(i, "mid");
                    coloredPixels++;
                }


                else if (pixel == Color.blue)
                {
                    spawnProjectile(i, "bot");
                    coloredPixels++;
                }
                else
                {
                    Debug.Log(pixel);
                    whitePixels++;
                }
                   
            }
        Debug.Log(string.Format("White pixels {0}, colored pixels {1}", whitePixels, coloredPixels));
        Debug.Log(wavePic.width);
        Debug.Log(wavePic.height);
    }

    void spawnProjectile(int x, string place)
    {
        GameObject pro = null;
        // spawn a top, mid or bot projectile according to the string and make it wait according to x
        pro = Instantiate(Projectile, transform.position, transform.rotation);
        pro.GetComponent<projectile>().waitTime = x;
        switch (place)
        {
            case "bot":
                pro.GetComponent<projectile>().setEnum("bot");
                break;
            case "mid":
                pro.GetComponent<projectile>().setEnum("mid");
                break;
            case "top":
                pro.GetComponent<projectile>().setEnum("top");
                break;

            default:
                Debug.Log("Well, shit");
               
                break;
        }
       
    }
}
