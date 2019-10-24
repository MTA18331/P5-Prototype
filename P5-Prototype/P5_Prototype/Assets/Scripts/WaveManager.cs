using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
   
    public static WaveManager instance = null;
  
    public int maxWaves;
    public float timeToReachTarget;

    public float waitTimeBeforeStart;
    public float waitTimeBeforeEnd;
    public GameObject EndCanvas;
    public GameObject ProjectileDad;
    public GameObject Projectile;
    public bool GenerateRandomWaves;
    public Texture2D[] waveTextures;

   
    public float flowMax;
    public float CurrentFlow = 1f;
    public float flowMin;
    private List<float> AverageFlowList = new List<float>();
   // [HideInInspector] public float AverageFlow;
    public float currentBlockCombo;
    public float currentMissCombo;
    
    
    int waitTime;

    int wavenumber=0; // husk at første wave er nummer 0
    public int[] blockamount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] missamount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float[] WaveFlow= { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public AudioClip[] MusicClip;

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
        StartCoroutine(Waves());
    }

    private void Update()
    {
        Flow();
    }


    void buildWave(Texture2D wavePic)
    {
        var weirdPixels = 0;
        var coloredPixels = 0;
        for (int i = 0; i < wavePic.width; i++)
        {
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
                    weirdPixels++;
                }
            }
        }
        //Debug.Log(string.Format("weird pixels {0}, colored pixels {1}", weirdPixels, coloredPixels));

    }

    void spawnProjectile(float x, string place)
    {
        //GameObject pro = null;
        GameObject pro = Instantiate(Projectile, transform.position, transform.rotation);
        pro.GetComponent<projectile>().waitTime = x* CurrentFlow;
        pro.GetComponent<projectile>().WaveNumber = wavenumber;
        pro.transform.SetParent(ProjectileDad.transform);
        
 
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

    void gameEnd()
    {
        CSV.instance.Save( blockamount, missamount, WaveFlow);
    }

    void Flow()
    {
        if (currentBlockCombo % 3 == 1)
        {
            if (CurrentFlow > flowMin)
            {
                CurrentFlow -= 0.05f;
                currentBlockCombo = 0;
                currentMissCombo=0;
              
            }
        }
        if (currentMissCombo % 6 ==1)
        {
            if (CurrentFlow < flowMax)
            {

            CurrentFlow += 0.05f;
            currentMissCombo=0;
             currentBlockCombo = 0;
            }
        }
    }

    IEnumerator Waves()
    {

        if (wavenumber == 0)
        {
            yield return new WaitForSeconds(waitTimeBeforeStart);
        }


        Debug.Log("Send out wave " + wavenumber + " based on flow of " + CurrentFlow);
        if (GenerateRandomWaves && wavenumber<maxWaves)
        {

            for (int i = 0; i < 10; i++)
            {
                int r = Random.Range(0, 2);
                if (r == 0)
                {
                    spawnProjectile(i, "top");
                }
                else if (r == 1)
                {
                    spawnProjectile(i, "mid");
                }
                else if (r == 2)
                {
                    spawnProjectile(i, "mid");
                }
                
                

            }
            WaveFlow[wavenumber] = CurrentFlow;
            wavenumber++;
        } else if (wavenumber < maxWaves && !GenerateRandomWaves)
        {

           buildWave(waveTextures[wavenumber]);
            WaveFlow[wavenumber] = CurrentFlow;
            wavenumber++;
        }else
        {
            yield return new WaitForSeconds(waitTimeBeforeEnd);
            EndCanvas.SetActive(true);
            gameEnd();
            StopCoroutine(Waves());
        }

       
        yield return new WaitForSeconds(CurrentFlow*10);
        StartCoroutine(Waves());
           

        }
    }

