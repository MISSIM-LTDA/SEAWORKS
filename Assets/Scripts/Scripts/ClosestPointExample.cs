using UnityEngine;

public class ClosestPointExample : MonoBehaviour
{
    public GameObject gameObjectA;
    public GameObject gameObjectB;

    private void Update()
    {
        Mesh meshA = gameObjectA.GetComponent<MeshFilter>().mesh;
        Mesh meshB = gameObjectB.GetComponent<MeshFilter>().mesh;

        Vector3[] verticesA = meshA.vertices;
        Vector3[] verticesB = meshB.vertices;

        Vector3 closestPointA = Vector3.zero;
        Vector3 closestPointB = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (Vector3 vertexA in verticesA)
        {
            foreach (Vector3 vertexB in verticesB)
            {
                float distance = Vector3.Distance(vertexA, vertexB);
                if (distance < closestDistance)
                {
                    closestPointA = vertexA;
                    closestPointB = vertexB;
                    closestDistance = distance;
                }
            }
        }

        Debug.Log("Closest point on object A: " + closestPointA);
        Debug.Log("Closest point on object B: " + closestPointB);
        Debug.Log("Closest distance: " + closestDistance);
    }
}
