using Obi;
using UnityEngine;

[RequireComponent(typeof(ObiFixedUpdater))]
public class FixedUpdaterConfigurator : MonoBehaviour
{
    private void Start()
    {
        FixScene();
    }
    public void FixScene() 
    {
        ObiFixedUpdater fixedUpdater = GetComponent<ObiFixedUpdater>();
        ObiSolver m_Solver = GetComponent<ObiSolver>();

        ObiSolver[] solvers = FindObjectsOfType<ObiSolver>(true);

        fixedUpdater.solvers.Clear();
        foreach (ObiSolver solver in solvers) {
           fixedUpdater.solvers.Add(solver);
        }
    }

}
