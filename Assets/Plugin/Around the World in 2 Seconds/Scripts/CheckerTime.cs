using UnityEngine;
using System.Collections;

public class CheckerTime : MonoBehaviour
{
	public bool isOut;

	void OnTriggerEnter2D(Collider2D col) 
	{
		if (col.name.Contains("CheckerTime"))
		{
			isOut = false;
		}
	}

	void OnTriggerExit2D(Collider2D col) 
	{
		if (col.name.Contains("CheckerTime"))
		{
			isOut = true;
		}
	}
}
