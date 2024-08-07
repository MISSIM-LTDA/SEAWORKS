using UnityEngine;
using Obi;

public class CraneController : MonoBehaviour
{
    ObiRope rope;
    ObiRopeCursor cursor;

	public KeyCode payUp = KeyCode.JoystickButton11;
	public KeyCode retrive = KeyCode.JoystickButton10;

	public float minLimitLenght = 6.5f;
	public float speed;
	void Start()
	{
        rope = GetComponent<ObiRope>();
        cursor = GetComponent<ObiRopeCursor>();
	}
	void Update() {
        if (Input.GetKey(payUp)){
            cursor.ChangeLength(rope.restLength + speed * Time.deltaTime);
        }
        else if (Input.GetKey(retrive) && rope.restLength > minLimitLenght) {
			cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);
		}
	}
}
