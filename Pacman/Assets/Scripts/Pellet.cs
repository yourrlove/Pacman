using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int point;
    protected virtual void Eat()
    {
        GameManager.Instance.PelletEaten(this);
    }

    // Trigger event: Pellet be eaten by Pacman
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Eat();
        }
    }
}
