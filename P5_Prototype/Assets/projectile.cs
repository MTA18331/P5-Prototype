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
    float LaunchAngle = 10f;
    Rigidbody rigid;
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
        rigid = GetComponent<Rigidbody>();
       // transform.rotation = Quaternion.LookRotation(rigid.velocity) * transform.rotation;
        StartCoroutine(GoTimer());
    }

    // Update is called once per frame
    void Update()
    {
  
        if (go)
        {
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startpos.transform.position, target.transform.position, t);

            //Launch();

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

    void Launch()
    {

        transform.rotation = Quaternion.LookRotation(rigid.velocity);
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(target.transform.position.x, 0.0f, target.transform.position.z);

        // rotate the object to face the target
        transform.LookAt(targetXZPos);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = target.transform.position.y - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rigid.velocity = globalVelocity;
       // bTargetReady = false;
    }
}
