using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer ScaredState;
    public SpriteRenderer TransformState;

    private bool eaten;

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        ScaredState.enabled = true;
        TransformState.enabled = false;
        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        base.Disable();
        body.enabled = true;
        eyes.enabled = true;
        ScaredState.enabled = false;
        TransformState.enabled = false;

    }

    private void Eaten()
    {
        eaten = true;
        body.enabled = false;
        eyes.enabled = true;
        ScaredState.enabled = false;
        TransformState.enabled = false;
        ghost.SetPosition(ghost.home.inside.position);
        ghost.home.Enable(duration);


    }

    private void Flash()
    {
        if (!eaten)
        {
            ScaredState.enabled = false;
            TransformState.enabled = true;
            TransformState.GetComponent<Animation>().Restart();
        }
    }

    private void OnEnable()
    {
        ScaredState.GetComponent<Animation>().Restart();
        ghost.movement.speedMultiplier = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f;
        eaten = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Find the available direction that moves farthest from pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is greater than the current
                // max distance then this direction becomes the new farthest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled)
            {
                Eaten();
            }
        }
    }
}
