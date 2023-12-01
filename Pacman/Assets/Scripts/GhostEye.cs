using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    private SpriteRenderer spriteRenderer;
    private Movement movement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<Movement>();
    }

    // Handle: eye ghost will follow the direction of its movement
    private void Update()
    {
        if (movement.curDirection == Vector2.up)
        {
            spriteRenderer.sprite = up;
        }
        else if (movement.curDirection == Vector2.down)
        {
            spriteRenderer.sprite = down;
        }
        else if (movement.curDirection == Vector2.left)
        {
            spriteRenderer.sprite = left;
        }
        else if (movement.curDirection == Vector2.right)
        {
            spriteRenderer.sprite = right;
        }
    }

}