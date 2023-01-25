using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShootController : NetworkBehaviour
{

    public float shootSpeed = 15f;
    public bool canShoot = true;
    public GameObject BulletPosition;
    public GameObject Bullet;
    public Camera maincam;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            maincam.GetComponent<AudioListener>().enabled = false;
        }
    }
    private void Update()
    {

        if (!isLocalPlayer) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                CmdSpawnBullet();
            }
        }
    }

    [Command]
    void CmdSpawnBullet()
    {
        GameObject newBullet = Instantiate(Bullet, BulletPosition.transform.position, transform.rotation);
        NetworkServer.Spawn(newBullet, connectionToClient);
        newBullet.GetComponent<Rigidbody>().AddForce(maincam.transform.forward * shootSpeed, ForceMode.Impulse);
    }
}
