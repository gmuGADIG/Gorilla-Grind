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


    /**
     * Reconnects every ground edge to the next one
     */
    public void VerifySubsections()
    {
        subsections = GetComponentsInChildren<Subsection>();



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
