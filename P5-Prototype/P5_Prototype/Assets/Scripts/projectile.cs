
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
    
    bool go = false;
    float timeToReachTarget;
    float t;
    
    enum ProjectileType { none, top, mid, bot };
    ProjectileType pt;

    public bool randomStartpos;
    public float LaunchAngle = 60f;
    Rigidbody rigid;
	Vector3 makePeak;
    

    // Start is called before the first frame update
    void Start()
    {
		makePeak = new Vector3(0.0f, 5.0f, 0.0f);

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
        startpos = GetStartPos();
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

		transform.position = (1.0f - t) * (1.0f - t) * startpos.transform.position + 2*(1-t)*t*(startpos.transform.position+makePeak)+(t*t)*target.transform.position;
		//transform.rotation = Quaternion.LookRotation(transform.position);
		transform.rotation = Quaternion.LookRotation(startpos.transform.position + makePeak);

	}

    GameObject GetStartPos()
    {
        if (randomStartpos)
        {
            try
            {
                GameObject[] startPositions = GameObject.FindGameObjectsWithTag("startPosition");
                int r = Random.Range(0, startPositions.Length);
                //Debug.Log("spawning from " + startPositions[r].name);
                return startPositions[r];
            }
            catch
            {
                Debug.LogError("Projectiles failed to find starting positions");
                return null;
            }
        } else
        {
            // alternativ ide: pil1->pos1, pil2->pos2 -> pil3->pos3 indtil igennem alle startpos, derefter pilx->pos1, pilx+1->pos2
            GameObject[] startPositions= GameObject.FindGameObjectsWithTag("startPosition");
            if (pt == ProjectileType.top)
            {
                return startPositions[2]; // top start pos
            }
            else if (pt == ProjectileType.mid)
            {
                return startPositions[1]; // mid start pos
            }
            else if (pt == ProjectileType.bot)
            {
                return startPositions[0]; // bottom start pos
            }
            else
            {
                Debug.Log("problems with findings starting posistions!");
                return null;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "shield"){
            
            go = false;
            //audiomanager.instance.PlayArrowBlock();
            WaveManager.instance.blockamount[WaveNumber] ++;
            WaveManager.instance.currentBlockCombo ++;
            WaveManager.instance.currentMissCombo = 0;
            transform.SetParent(collision.gameObject.transform);
            
            rigid.isKinematic = true;
            Destroy(gameObject, 10);
        
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


