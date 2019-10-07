using UnityEngine;
using System.Collections;

public class metalimpact: MonoBehaviour
{

    public float despawnTime;
    public float lightDuration;

    public Light lightObject;

  


    private void OnEnable()
    {
        lightObject.GetComponent<Light>().enabled = true;
        StartCoroutine(DespawnTimer());
        StartCoroutine(LightFlash());
    }

    IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        gameObject.SetActive(false);
    }

    IEnumerator LightFlash()
    {
        yield return new WaitForSeconds(lightDuration);
        lightObject.GetComponent<Light>().enabled = false;
    }
}