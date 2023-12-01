using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public float speed = 7f;
    public float speedMultiplier = 1f;
    public Vector3 startingPosition;
    public Vector2 initialDirection;
    public Vector2 curDirection;
    private Vector2 nextDirection;
    public Rigidbody2D body;

    public Vector2 CurDirection { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        obstacleLayer = LayerMask.GetMask("Walls");
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetState();
    }

    // Reset to inital Direction and Position when start new game or new round
    public void ResetState()
    {
        enabled = true;
        this.transform.position = startingPosition;
        this.curDirection = initialDirection;
        this.nextDirection = Vector2.zero;
    }

    public void ResetPosition()
    {
        this.transform.position = startingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 curPosition = body.position;
        Vector2 move = curDirection * speed * speedMultiplier * Time.fixedDeltaTime;
        body.MovePosition(curPosition + move);
    }

    // Check whether this direction is available or is blocked
    // allow: use by ghost in home when they are allowed to get out of home
    public void SetDirection(Vector2 direction, bool allow = false)
    {
        if (!WallBlock(direction) || allow)
        { 
            curDirection = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            // if this direction is block, store it in nextDirection
            // nextDirection will be checked in each frame, if that direction is not blocked
            // Object will follow immediately to that direction
            nextDirection = direction;
        }
    }

    private bool WallBlock(Vector2 direction)
    {
        Vector2 curPosition = transform.position;
        RaycastHit2D hitWall = Physics2D.BoxCast(curPosition, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hitWall.collider != null;
    }
}
