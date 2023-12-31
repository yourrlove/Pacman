using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public List<Vector2> availableDirections { get; private set; }
    [SerializeField] private List<Transform> neighbourNodes;

    private void Awake()
    {
        availableDirections = new List<Vector2>();
    }


    private void Start()
    {
        availableDirections.Clear();
        // We determine if the direction is available by box casting to see if
        // we hit a wall. The direction is added to list if available.
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }
    //public List<Transform> GetNeighbourNode()
    //{
    //    List<Transform> result = this.neighbourNodes;
    //    return result;
    //}

    //public Vector3 GetNodePosition()
    //{
    //    return this.transform.position;

    //}

    private void CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        // If no collider is hit then there is no obstacle in that direction
        if (hit.collider == null)
        {
            this.availableDirections.Add(direction);
        }
    }
}
