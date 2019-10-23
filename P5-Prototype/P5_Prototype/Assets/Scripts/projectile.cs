
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    
    public float waitTime;
    public int WaveNumber;
    public GameObject startpos;
    public GameObject  Peak; 
    public GameObject target;
    
    GameObject[] startPositions;
    bool go = false;
    float timeToReachTarget;
    float t;
    
    enum ProjectileType { none, top, mid, bot };
    ProjectileType pt;
    
    public float LaunchAngle = 60f;
    Rigidbody rigid;
    
    
    

    // Start is called before the first frame update
    void Start()
    {

        if (pt == ProjectileType.top)
        {
            target = GameObject.FindGameObjectWithTag("PTop");
        }
        else if (pt == ProjectileType.mid)
        {
            target = GameObject.FindGameObjectWithTag("PMid");
        }
        else if (pt == ProjectileType.bot)
        {
            target = GameObject.FindGameObjectWithTag("PBot");
        } else {
            Debug.Log("problems with findings targets!");
        }
        startpos = RandomStartPos();
        timeToReachTarget = WaveManager.instance.timeToReachTarget;
        transform.position = startpos.transform.position;
        Peak = startpos.transform.GetChild(0).gameObject;
        GameObject.FindGameObjectWithTag("Peak");
        rigid = GetComponent<Rigidbody>();
       // transform.rotation = Quaternion.LookRotation(rigid.velocity) * transform.rotation;
        //transform.rotation = Quaternion.LookRotation(transform.position);
        //transform.rotation = Quaternion.LookRotation(-rigid.velocity);
        StartCoroutine(GoTimer());
    }

    // Update is called once per frame
    void Update()
    {
  
        if (go)
        {
           //audiomanager.instance.PlayArrowStarting();
            t += Time.deltaTime / timeToReachTarget;
           //transform.position = Vector3.Lerp(startpos.transform.position, target.transform.position, t);
            
            Launch();

        }
        
       
    }

    IEnumerator GoTimer()
    {
        yield return new WaitForSeconds(waitTime);
        go = true;
        //MeshRenderer m = GetComponent<MeshRenderer>();
        //m.enabled = true;
    }

    public void setEnum(string enumSet)
    {
        if (enumSet == "top"){
            pt = ProjectileType.top;
        }
        else if (enumSet == "mid"){
            pt = ProjectileType.mid;
        }
        else if (enumSet == "bot"){
            pt = ProjectileType.bot;
        }
        else{
            pt = ProjectileType.none;
        }
    }

    void Launch()
    {
        
        //rigid.velocity = -startpos.transform.forward*5;
        transform.position = (1.0f - t) * (1.0f - t) * startpos.transform.position + 2*(1-t)*t*Peak.transform.position+(t*t)*target.transform.position;
        //transform.rotation = Quaternion.LookRotation(-rigid.velocity);
        Vector3 direction = new Vector3(0,0,0);
        direction.x = transform.position.x - target.transform.position.x;
        direction.y = transform.position.y - target.transform.position.y;
        direction.z = transform.position.z - target.transform.position.z;

        float angle = Mathf.Atan2 (-direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = (0, angle, 0);
        Vector3 ops= new Vector3(0, angle, 0)
        transform.rotation = Quaternion.LookRotation( ops );
    }

    GameObject RandomStartPos()
    {
        try{
            startPositions = GameObject.FindGameObjectsWithTag("startPosition");
            int r = Random.Range(0, startPositions.Length);
            //Debug.Log("spawning from " + startPositions[r].name);
            return startPositions[r];
        }
        catch{
            Debug.LogError("Projectiles failed to find starting positions");
            return null;
        }    
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "shield"){
            Debug.Log("Executed");
            go = false;
            //audiomanager.instance.PlayArrowBlock();
            WaveManager.instance.blockamount[WaveNumber] ++;
            WaveManager.instance.currentBlockCombo ++;
            WaveManager.instance.currentMissCombo = 0;
            //transform.SetParent(collision.gameObject.transform);
            //Destroy(gameObject, 10);
            
            transform.SetParent(collision.gameObject.transform);
            Destroy(gameObject, 15);
        }
        else if (collision.gameObject.tag == "PTop" || collision.gameObject.tag == "PMid" || collision.gameObject.tag == "PBot")
        {
            //audiomanager.instance.PlayHurt();
            WaveManager.instance.missamount[WaveNumber] ++;
            WaveManager.instance.currentBlockCombo = 0;
            WaveManager.instance.currentMissCombo ++;
            go = false;
            Destroy(gameObject);
        }


    }
}


