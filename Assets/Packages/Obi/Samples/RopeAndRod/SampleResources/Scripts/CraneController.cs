using UnityEngine;
using Obi;

public class CraneController : MonoBehaviour
{
    private ObiRope rope;
    private ObiRopeCursor cursor;

	public KeyCode extend;
	public KeyCode decrease;

	public float speed;

	void Start()
	{
		cursor = GetComponentInChildren<ObiRopeCursor>();
		rope = cursor.GetComponent<ObiRope>();
	}
	void FixedUpdate()
	{
		if (Input.GetKey(decrease) && rope.restLength > 6.5f){
			cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);
		}

		else if (Input.GetKey(extend)){
			cursor.ChangeLength(rope.restLength + speed * Time.deltaTime);
		}

	}
}
