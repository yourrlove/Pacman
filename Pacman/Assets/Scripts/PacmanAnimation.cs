using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanAnimation : Animation
{ 
    public Sprite[] deathAnimation;
    public float deathAnimationTime = 2.0f;


    protected override void Start()
    {
        if(loop)
        {
            // Reapt the animation
            InvokeRepeating(nameof(Advance), activeAnimationTime, activeAnimationTime);
        } else
        {
            Advance();
        }

    }
    // Switch to death Animation for Pacman 
    public void Die()
    {
        sprites = deathAnimation;
        loop = false;
        animationFrame = 0;
    }
    // Switch to active Animation for Pacman 
    public void Revival()
    {
        sprites = activeAnimation;
        loop = true;
        animationFrame = 0;
    }
}
