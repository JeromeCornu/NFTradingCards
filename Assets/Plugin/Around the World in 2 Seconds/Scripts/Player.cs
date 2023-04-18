using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public GameManager gm;
	public float jumoForce = 470;
	//public float jumoForce = 470;

	public AudioClip jump;
	public AudioClip dead;

	Animator anim;

	public bool canJump = true;

	void Start()
	{
		canJump = true;
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		if (gm.GameStart && canJump)
		{
			if (Input.GetMouseButtonDown(0))
			{
				GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumoForce);
				anim.Play("PlayerJump");
				GetComponent<AudioSource>().clip = jump;
				GetComponent<AudioSource>().Play();
				canJump = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) 
	{
		if (col.tag.Contains("Enemy"))
		{
			Camera.main.GetComponent<CameraShake>().Shake();
			Time.timeScale = 0;
			gm.GameStart = false;
			canJump = false;
			anim.updateMode = AnimatorUpdateMode.UnscaledTime;
			anim.Play("Dead");
			GetComponent<AudioSource>().clip = dead;
			GetComponent<AudioSource>().Play();
		}
		else if (col.tag.Contains("Ground"))
		{
			canJump = true;
		}
	}

	void GameOver()
	{
		gm.GameOver();
	}
}