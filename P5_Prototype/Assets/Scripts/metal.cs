using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metal : MonoBehaviour
{
    public int hitamount;
    public GameObject metal1;
    public GameObject metal2;
    public GameObject metal3;
    public GameObject metalhiteffect;
    public Collider col;
    public bool canCollide = true;
    ///public GameObject sparkprefab;
    

    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if (hitamount == 5)
        {
            metal1.SetActive(false);
            metal2.SetActive(true);
        }
        if (hitamount == 10)
        {
            metal2.SetActive(false);
            metal3.SetActive(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
   
    {

       
         if (canCollide) {
            if (collision.gameObject.tag == "Hammer")
            {
                hitamount++;
                Debug.Log(collision.gameObject.name);
                metalhiteffect.SetActive(true);
                canCollide = false ;
            }
           
         }
       

    }
    void OnCollisionExit(Collision collisionInfo)
    {
        canCollide = true;
    }
}
