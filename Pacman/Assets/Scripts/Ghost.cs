using System.Threading;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;

    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        frightened = GetComponent<GhostFrightened>();
        chase = GetComponent<GhostChase>();
        scatter = GetComponent<GhostScatter>();
        home = GetComponent<GhostHome>();
    }

    private void Start()
    {
        ResetState();
    }


    public void ResetState(bool newRound = false)
    {
        gameObject.SetActive(true);
        movement.ResetState();
        
        // At the first time, all ghost will in Scatter state
        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        // Used for ghosts which are not be set in Home at first time
        if (home != initialBehavior)
        {
            home.Disable();
        }

        // Trigger special initialBehavior for some ghosts
        // Ex: Inky, Coldy, Pinky will stay in home at first, except Blinky
        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }

    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            movement.SetDirection(Vector2.zero);
        }
    }

    // Trigger Event:
    // + Ghost be eaten by Pacman if they are in frightented state
    // + Ghost eats Pacman if they are not in frightented state
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled)
            {
                GameManager.Instance.GhostEaten(this);
                chase.Disable();
                scatter.Enable();
            }
            else
            {
                GameManager.Instance.PacmanEaten();
            }
        }
    }
}
