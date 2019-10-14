using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Shield : MonoBehaviour
{
    [SerializeField] private SteamVR_Behaviour_Pose controllerPose;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = controllerPose.gameObject.transform.position;
        gameObject.transform.rotation = controllerPose.gameObject.transform.rotation;
    }

    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.tag == "projectile")
        {
            Destroy(collision.gameObject);

        }
    }


}
