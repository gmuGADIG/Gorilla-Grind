using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
[ExecuteInEditMode]
public class GroundSection : MonoBehaviour
{
    [HideInInspector]
    public Subsection[] subsections;

    
    public Subsection mainsection;

    public Vector2 startPoint { get => mainsection.startPoint; }

    public Vector2 endPoint { get => mainsection.endPoint; }

    public float heightDiff => mainsection?.heightDiff ?? 0;


    /**
     * Reconnects every ground edge to the next one
     */
    public void VerifySubsections()
    {
        subsections = GetComponentsInChildren<Subsection>();

        if(subsections.Length == 0)
        {
            Debug.Log(name + " doesn't have any subsections.");

            //This code doesn't work because prefabs or smth.
            
            //GameObject temp = new GameObject("Main subsection");
            //temp.AddComponent<Subsection>();
            //foreach (GroundEdge i in transform.GetComponentsInChildren<GroundEdge>())
            //{
            //    i.transform.SetParent(temp.transform);
            //}
            //temp.transform.SetParent(transform);

            //subsections = new Subsection[] { temp.GetComponent<Subsection>() };

        }


    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        EditorApplication.hierarchyChanged += VerifySubsections;
        if (mainsection == null) mainsection = GetComponentInChildren<Subsection>();
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= VerifySubsections;
    }


    void Update()
    {
        
    }


}
