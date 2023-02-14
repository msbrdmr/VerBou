using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;
public class PlayerHealthScript : NetworkBehaviour
{
    [SyncVar] public int Health = 100;
    public TextMeshProUGUI healthtext;
    public void GetDamage(int dmg)
    {
        Health -= dmg;
        if (Health < 0)
        {
            Debug.Log("Died");
            Health = 100;
            //respawn here
        }

    }

    private void Update()
    {
        if (!isLocalPlayer) { return; }
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (healthtext == null)
            {
                healthtext = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
            }

            healthtext.text = Health.ToString();
        }
    }

}
