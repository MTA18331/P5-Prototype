using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public int waitTime;
    bool go = false;
    public GameObject startpos;
    public GameObject target;
    float timeToReachTarget;
    float t;
    enum ProjectileType { none, top, mid, bot };
    ProjectileType pt;
    // Start is called before the first frame update
    void Start()
    {
        if (pt == ProjectileType.top)
        {
            startpos = GameObject.FindGameObjectWithTag("StartTopProjectile");
            target = GameObject.FindGameObjectWithTag("PTop");
        }
        if (pt == ProjectileType.mid)
        {
            startpos = GameObject.FindGameObjectWithTag("StartMidProjectile");
            target = GameObject.FindGameObjectWithTag("PMid");
        }
        if (pt == ProjectileType.bot)
        {
            startpos = GameObject.FindGameObjectWithTag("StartBotProjectile");
            target = GameObject.FindGameObjectWithTag("PBot");

        }
        if (pt == ProjectileType.none)
        {
            Debug.Log("Enum didnt work dude");
        }

        timeToReachTarget = WaveManager.instance.timeToReachTarget;
        transform.position = startpos.transform.position;
        
        StartCoroutine(GoTimer());
    }

    // Update is called once per frame
    void Update()
    {
  
        if (go)
        {
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startpos.transform.position, target.transform.position, t);
        }
        
       
    }

    IEnumerator GoTimer()
    {
        yield return new WaitForSeconds(waitTime);
        go = true;
        MeshRenderer m = GetComponent<MeshRenderer>();
        m.enabled = true;
    }

    public void setEnum(string enumSet)
    {
        if (enumSet== "top")
        {
            pt = ProjectileType.top;
        }
        else if (enumSet == "mid")
        {
            pt = ProjectileType.mid;
        }
        else if (enumSet == "bot")
        {
            pt = ProjectileType.bot;
        }
        else 
        {
            pt = ProjectileType.none;
        }

    }
}
