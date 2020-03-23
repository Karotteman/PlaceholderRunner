using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager;
    public LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collectible")
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            gameManager.Score++;
            levelManager.maxDanger += .5f;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else if(other.gameObject.tag == "KillZone")
        {
            gameManager.GameOver();
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
