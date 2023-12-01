using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;
    private float totalTime = 0.5f;
    private float elapsed = 0.0f;


    //  Handle Event:  Ghost bump into wall will be bounce back
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            this.ghost.movement.SetDirection(-this.ghost.movement.curDirection);
        }
    }

    /// <summary>
    /// Not Allow ghost to gout
    /// </summary>
    private void OnEnable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Allow ghost to go out
    /// </summary>
    private void OnDisable()
    {
        if(this.gameObject.activeSelf)
        {
            StartCoroutine(ExitTransition());
        }
    }


    /// <summary>
    ///  Handle Event:  Ghost be hold in their home
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExitTransition()
    {
        this.ghost.movement.SetDirection(Vector2.up, true);
        this.ghost.movement.body.isKinematic = true;
        this.ghost.movement.enabled = false;

        Vector3 position = this.transform.position;

        // Hanlde the animation when ghost travel to the door - inside position to be ready go outside after an elapsed time

        while(elapsed < totalTime)
        {
            Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / totalTime);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;

        // Hanlde the animation when ghost go out of home - outside position after an elapsed time
        while (elapsed < totalTime)
        {
            Vector3 newPosition = Vector3.Lerp(this.inside.position, this.outside.position, elapsed / totalTime);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // After going out of home, choose random direction for them
        this.ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true);
        this.ghost.movement.body.isKinematic = false;
        this.ghost.movement.enabled = true;

    }
}
