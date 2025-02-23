using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVisual : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyDelay());
    }

    //wait to destroy visual after a second
    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
