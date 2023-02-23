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
    public Vector2 boundSize;

    /**
     * Holds the edge collider
     */
    public EdgeCollider2D edgeCollider;

    
    public Vector2 startPoint { 
        get {
            return edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[0]);
            //return edgeCollider.points[0]; 

        } 
    }
    public Vector2 endPoint { get { return edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[edgeCollider.pointCount - 1]);  } }
    public GroundEdge previous;
    public GroundEdge next;

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
