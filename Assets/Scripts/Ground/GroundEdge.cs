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
    [HideInInspector]
    public Vector2 boundSize;

    /**
     * Holds the edge collider
     */
    public EdgeCollider2D edgeCollider;

    public static bool shouldRenderEdge;

    [SerializeField]
    [Header("Check box to designate a edge as a 'gap' in the world")]
    public bool noCollision;

    public Vector2 startPoint { 
        get {
            return edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[0]);
            //return edgeCollider.points[0]; 

        } 
    }
    public Vector2 endPoint { get { return edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[edgeCollider.pointCount - 1]);  } }
    [Space]
    [Header("Edges will follow the first node in series")]
    public GroundEdge previous;
    public GroundEdge next;

    public static Color solidColor = Color.yellow;
    public static Color gapColor = Color.blue;



    // Start is called before the first frame update
    void Start()
    {
        edgeCollider.enabled = !noCollision;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldRenderEdge)
        {
            for (int i = 0; i < edgeCollider.pointCount - 1; i++)
            {
                Debug.DrawLine(edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[i]),
                    edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[i + 1]), noCollision ? gapColor : solidColor);
            }
        }
    }

}
