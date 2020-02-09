using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("DestroyMe");
    }
    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }
}
