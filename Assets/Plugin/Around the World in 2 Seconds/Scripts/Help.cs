using UnityEngine;
using System.Collections;

public class Help : MonoBehaviour 
{
	public Animator help;

	void OnMouseDown()
	{
		transform.localScale = new Vector3(0.9f,0.9f,1);
	}
	
	void OnMouseUp()
	{
		transform.localScale = new Vector3(1,1,1);

		help.Play("Help");
	}
}
