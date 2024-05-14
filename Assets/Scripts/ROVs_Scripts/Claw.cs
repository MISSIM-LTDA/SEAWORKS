using UnityEngine;

public class Claw : MonoBehaviour
{
    [SerializeField] private Transform moveObject;

    [SerializeField] private KeyCode clockWise;
    [SerializeField] private KeyCode CounterClockWise;
    [Space(10)]

    [SerializeField] private float speed;
    [Space(10)]

    [SerializeField] private float maxLimit;
    [SerializeField] private float minLimit;
    [Space(10)]

    [SerializeField] private bool translate;
    [SerializeField] private bool rotate;
    [Space(10)]

    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private bool z;

    float rotateAngle;
    void Update()
    {
        if (Input.GetKey(CounterClockWise)){
            if (rotate) {
                rotateAngle++;
                if (rotateAngle < maxLimit){
                    if (x) { moveObject.Rotate(speed, 0.0f, 0.0f); }
                    else if (y) { moveObject.Rotate(0.0f, speed, 0.0f); }
                    else if (z) { moveObject.Rotate(0.0f, 0.0f, speed); }
                }
                else{
                    rotateAngle = (maxLimit - 1);
                }
            }
            else if (translate) {
                rotateAngle += speed;

                if (rotateAngle >= maxLimit){
                    rotateAngle = maxLimit + speed;
                }

                else{
                    if (x) { moveObject.Translate(speed, 0.0f, 0.0f); }
                    else if (y) { moveObject.Translate(0.0f, speed, 0.0f); }
                    else if (z) { moveObject.Translate(0.0f, 0.0f, speed); }
                }
            }
        }
        else if (Input.GetKey(clockWise)){
            if (rotate) {
                rotateAngle--;
                if (rotateAngle > minLimit){
                    if (x) { moveObject.Rotate(-speed, 0.0f, 0.0f); }
                    else if (y) { moveObject.Rotate(0.0f, -speed, 0.0f); }
                    else if (z) { moveObject.Rotate(0.0f, 0.0f, -speed); }
                }
                else{
                    rotateAngle = minLimit + 1;
                }
            }
            else if (translate) {
                rotateAngle -= speed;
                if (rotateAngle >= minLimit){
                    if (x) { moveObject.Translate(-speed, 0.0f, 0.0f); }
                    else if (y) { moveObject.Translate(0.0f, -speed, 0.0f); }
                    else if (z) { moveObject.Translate(0.0f, 0.0f, -speed); }
                }
                else{
                    rotateAngle = minLimit - speed;
                }
            }
        }
    }
}
