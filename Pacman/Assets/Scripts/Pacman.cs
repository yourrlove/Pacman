using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    private PacmanAnimation deathSequence;
    private SpriteRenderer sprite;
    private Movement movement;
    private new Collider2D collider;

    // Start is called before the first frame update
    private void Start()
    {
        deathSequence = GetComponent<PacmanAnimation>();
        sprite = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get direction from player
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up);
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right);
        }


        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(movement.curDirection.y, movement.curDirection.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            movement.SetDirection(Vector2.zero);
        }
    }

    // Hanlde Pacman reset state
    public void ResetState()
    {
        enabled = true;
        sprite.enabled = false;
        deathSequence.Revival();
        sprite.enabled = true;


        collider.enabled = true;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    // Handle Pacman deatth event
    public void DeathSequence()
    {
        deathSequence.Die();
        enabled = false;
        collider.enabled = false;
        movement.enabled = false;
    }
}