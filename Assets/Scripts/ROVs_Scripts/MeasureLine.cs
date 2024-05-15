using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MeasureLine : MonoBehaviour
{
    private LineRenderer line;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float width;
    [SerializeField] private float distance;

    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();

        line.startColor = Color.black;
        line.endColor = Color.black;

        line.startWidth = width;
        line.endWidth = width;

        line.positionCount = 2;
        line.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPointPosition = startPoint.position;
        Vector3 endPointPosition = endPoint.position;

        //For drawing line in the world space, provide the x,y,z values
        line.SetPosition(0, startPointPosition); //x,y and z position of the starting point of the line
        line.SetPosition(1, endPointPosition); //x,y and z position of the end point of the line

        distance = Vector3.Distance(startPointPosition, 
            endPointPosition) * 0.15f;
    }
}