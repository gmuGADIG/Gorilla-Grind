using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

    public static bool shouldRenderEdge = true;

    [SerializeField]
    [Header("Check box to designate a edge as a 'gap' in the world")]
    public bool noCollision;

    public Vector2 startPoint { 
        get {
            return edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[0]);
            //return edgeCollider.points[0]; 

        } 
    }
    public Vector2 endPoint { 
        get { 
            return edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[edgeCollider.pointCount - 1]);  
        } 
    }

    public float heightDiff => edgeCollider.points[edgeCollider.pointCount - 1].y - edgeCollider.points[0].y;

    public bool renderingSprite
    {
        get => GetComponent<SpriteRenderer>().sprite != null;
    }

    public bool renderingLine { get => !renderingSprite && !noCollision; }

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
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<LineRenderer>().enabled && !GetComponent<SpriteRenderer>().enabled) {
            GetComponent<LineRenderer>().enabled = renderingLine;
            GetComponent<SpriteRenderer>().enabled = renderingSprite;
        }
        
        if(renderingLine)
        {

            RenderLine();
        }
    }

    //This only renders if the sprite isn't set
    public void RenderLine()
    {
        LineRenderer lrender = GetComponent<LineRenderer>();
        if (lrender != null)
        {
            lrender.positionCount = edgeCollider.points.Length;
            //lrender.material.color = lrender.startColor = lrender.endColor = noCollision ? new Color(1, 1, 1, 0) : Color.yellow;
            //lrender.startWidth = lrender.endWidth = .25f;
            lrender.SetPositions(Utils.GetWorldPoints(Utils.Vec2ArrToVec3Arr(edgeCollider.points), edgeCollider.gameObject));
        }
    }

    [Obsolete("Trying to replace this with a more modular set of methods")]
    public void SnapEdge()
    {
        //Dont want cyclical links
        if (previous == this)
        {
            previous = null;
        }
        if (next == this)
        {
            next = null;
        }

        if (next != null)
        {
            next.previous = this;
            //edgeCollider.adjacentEndPoint = edgeCollider.transform.worldToLocalMatrix.MultiplyPoint(groundEdge.next.startPoint);
        }

        if (previous != null)
        {

            transform.position += (Vector3)(previous.endPoint - startPoint);

            //edgeCollider.adjacentStartPoint = edgeCollider.transform.worldToLocalMatrix.MultiplyPoint(groundEdge.previous.endPoint);

            //Plan on creating a visual tool to automatically connect and disconnect edges
            if (previous.next == null)
            {
                previous = null;
            }
        }
    }
    //Moves previous edges to connect to startPoint
    public void SnapPreviousEdges()
    {
        if (previous == this) previous = null;
        if (previous == null) return;
        previous.transform.position += (Vector3)(startPoint - previous.endPoint);
        previous.RenderLine();
        previous.SnapPreviousEdges();//propogate changes
    }

    public void SnapNextEdges()
    {
        if (next == this) next = null;
        if (next == null) return;
        next.transform.position += (Vector3)(endPoint - next.startPoint);
        next.RenderLine();
        next.SnapNextEdges();//propogate changes
    }

    public void SnapSurroundingEdges()
    {
        //Debug.Log(gameObject.name);
        SnapPreviousEdges();
        SnapNextEdges();
    }
    //void OnDrawGizmos()
    //{

    //    if (shouldRenderEdge)
    //    {
    //        Gizmos.color = noCollision ? gapColor : solidColor;
    //        for (int i = 0; i < edgeCollider.pointCount - 1; i++)
    //        {
    //            Gizmos.DrawLine(edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[i]),
    //                edgeCollider.transform.localToWorldMatrix.MultiplyPoint(edgeCollider.points[i + 1]));
    //        }
    //    }
    //}
}
