using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiParticleRenderer))]
[ExecuteInEditMode]
public class PaintParticleGroupsRenders : MonoBehaviour
{
    public GameObject rope;
    public ObiRope actor;
    public ObiSolver solver;

    public int activeParticles;

    [Range(0,23)]
    public int aux;

    public int numberOfParticle; 
    // Start is called before the first frame update
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        rope = this.gameObject;
        actor = rope.GetComponent<ObiRope>();
        activeParticles = actor.activeParticleCount;
        
        if(aux > 0 && aux < actor.activeParticleCount+1) 
        {
            for(int i = 0; i < actor.activeParticleCount; i++)
            {
                solver.colors[actor.solverIndices[i]] = Color.white;
            }
            solver.colors[actor.solverIndices[aux - 1]] = Color.red;
        }

        else 
        {
            for (int i = 0; i < actor.activeParticleCount; i++)
            {
                solver.colors[actor.solverIndices[i]] = Color.white;
            }
        }
    }
}
