using UnityEngine;
using Obi;

[ExecuteInEditMode]
public class CableLenght : MonoBehaviour
{
    private ObiRope obiRope;
    private ObiRod obiRod;

    [SerializeField] private float lenght;
    void Start()
    {
        obiRope = GetComponent<ObiRope>();
        obiRod = GetComponent<ObiRod>();
    }
    void Update()
    {
        if (obiRope) {
            lenght = obiRope.CalculateLength();
        }

        else if (obiRod) {
            lenght = obiRod.CalculateLength();
        }

        lenght *=  0.15f;
    }
}
