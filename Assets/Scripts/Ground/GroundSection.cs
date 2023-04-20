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

    public Vector2 startPoint { get => mainsection != null ? mainsection.startPoint : GetComponentInChildren<GroundEdge>().startPoint; }

    public Vector2 endPoint { get => mainsection != null ? mainsection.startPoint : GetComponentInChildren<GroundEdge>().endPoint; }

    public float heightDiff => mainsection != null ? mainsection.heightDiff : 0;


    /**
     * Reconnects every ground edge to the next one
     */
    public void VerifySubsections()
    {
        subsections = GetComponentsInChildren<Subsection>();

        if(subsections.Length == 0)
        {
            //Having no subsections can cause weird errors when editing so logging this as an error
            Debug.LogError(name + " doesn't have any subsections.");

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
        #if UNITY_EDITOR
        EditorApplication.hierarchyChanged += VerifySubsections;
        #endif
        if (mainsection == null) mainsection = GetComponentInChildren<Subsection>();
    }

    private void OnDisable()
    {
        #if UNITY_EDITOR
        EditorApplication.hierarchyChanged -= VerifySubsections;
        #endif
    }


    void Update()
    {
        
    }


}
