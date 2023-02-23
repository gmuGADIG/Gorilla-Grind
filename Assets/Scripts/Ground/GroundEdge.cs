using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Holds data about a ground edge. Edges themselves are prefabs that will get combined into chunks of the level.
 */
public class GroundEdge : MonoBehaviour
{
    /**
     * Represents the full bounds of the edge tile (length and height)
     */
    public Bounds bounds;

    /**
     * Holds the edge collider
     */
    public EdgeCollider2D groundEdge;

    
    public Vector2 startPoint { get { return groundEdge.points[0]; } }

    /** Whether to diplay the full bounds of this chunk */
    public bool showBounds;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
