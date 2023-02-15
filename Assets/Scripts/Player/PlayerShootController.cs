using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerShootController : NetworkBehaviour
{
    //will shoot raycast from center of camera
    public Camera fpsCam;
    public GameObject playerarms;
    [Range(0, 100)] public float range = 1000;

    private void Start()
    {
    }
    private void Update()
    {
        if (isLocalPlayer && Input.GetMouseButtonDown(0))
        {
            //shoot
            CmdShoot();

        }
    }

    [Command]
    public void CmdShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            var playerHealth = hit.collider.gameObject.GetComponent<PlayerHealthScript>();
            Debug.Log(hit.transform.name);
            playerarms.GetComponent<Animator>().SetTrigger("FireWeapon");
            if (playerHealth)
            {
                playerHealth.GetDamage(10);
            }

        }
    }
}
