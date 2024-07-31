using UnityEngine;
using Obi;

public class CraneController : MonoBehaviour
{
    private ObiRope rope;
    private ObiRopeCursor cursor;

	public KeyCode payOut = KeyCode.JoystickButton11;
	public KeyCode reelIn = KeyCode.JoystickButton10;

	public float minLenght = 6.5f;

	public float speed = 2.0f;
	void Start()
	{
        rope = GetComponent<ObiRope>();
        cursor = GetComponent<ObiRopeCursor>();
	}
	void Update()
	{
		if (Input.GetKey(reelIn)) {
			if (rope.restLength > minLenght) {
                cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);
            }
		}

		else if (Input.GetKey(payOut)) {
			cursor.ChangeLength(rope.restLength + speed * Time.deltaTime);
		}
	}
}
