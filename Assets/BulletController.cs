using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletController : NetworkBehaviour
{

    // public ParticleSystem particle;
    // public GameObject nozzle;
    private void Start()
    {
        StartCoroutine(DestroyObjectAfterTime(2f));
    }


    // Executed only on the server

    [Command]
    public void CmdDestroy(GameObject bullet)
    {
        Destroy(bullet);
    }
    IEnumerator DestroyObjectAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        CmdDestroy(gameObject);
    }

}
