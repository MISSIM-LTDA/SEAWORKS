using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class CraneController : MonoBehaviour
{

	ObiRopeCursor cursor;
	ObiRope rope;
	public KeyCode Pagar;
	public KeyCode Recolher;
	public float speed1;


	// Use this for initialization
	void Start()
	{
		cursor = GetComponentInChildren<ObiRopeCursor>();
		rope = cursor.GetComponent<ObiRope>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(Recolher))
		{
			if (rope.restLength > 6.5f)
				cursor.ChangeLength(rope.restLength - speed1 * Time.deltaTime);
		}

		if (Input.GetKey(Pagar))
		{
			
			cursor.ChangeLength(rope.restLength + speed1 * Time.deltaTime);
		}


	}
}
