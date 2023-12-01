using UnityEngine;

public class Animation : MonoBehaviour
{
    protected Sprite[] sprites;
    public Sprite[] activeAnimation;
    public float activeAnimationTime = 0.25f;
    public bool loop = true;

    protected SpriteRenderer spriteRenderer;
    protected int animationFrame;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = activeAnimation;
    }

    protected void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    protected void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    protected virtual void Start()
    {
        InvokeRepeating(nameof(Advance), activeAnimationTime, activeAnimationTime);
    }

    // Handle all Objects animation in each frame
    protected void Advance()
    {
        if (!spriteRenderer.enabled)
        {
            return;
        }

        animationFrame++;

        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    public void Restart()
    {
        animationFrame = -1;

        Advance();
    }

}