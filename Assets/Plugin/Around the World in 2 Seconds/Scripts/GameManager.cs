using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject menu;

	public bool GameStart;

	public CheckerTime ct;

	Animator anim;
	
	void Start()
	{
		anim = GetComponent<Animator>();
		GameStart = false;
	}

	void Update()
	{
		if (GameStart && Time.fixedTime % 2 == 0 && ct.isOut)
		{
			anim.speed = Random.Range(1,3);
		}
	}

	void OnMouseUp()
	{
		if (!GameStart)
		{
			GameStart = true;

			menu.SetActive(false);
			anim.Play("Earth");
		}
	}

	public void GameOver()
	{

		menu.SetActive(true);
		anim.Play("Idle");

		Time.timeScale = 1;
		Application.LoadLevel(0);
	}
}