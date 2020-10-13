using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collectible")
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            gameManager.Score++;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else if(other.gameObject.tag == "KillZone")
        {
            gameManager.GameOver();
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
