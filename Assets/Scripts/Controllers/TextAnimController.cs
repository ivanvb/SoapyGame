using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimController : MonoBehaviour {

	public GameObject startText;
	public GameObject timeOverText;
	public GameObject victoryText;

	public void PlayStartAnim()
	{
		StartCoroutine(TextAnim(startText, 2.5f, 0.45f));
	}

	public void PlayTimeOvertAnim()
	{
		StartCoroutine(TextAnim(timeOverText, 0.2f, 1f));
	}

	public void PlayVictoryAnim()
	{
		StartCoroutine(TextAnim(victoryText, 0.9f, 1f));
	}

	private IEnumerator TextAnim(GameObject text, float delay1, float delay2)
	{
		GameObject myGame = Instantiate(text, new Vector3(10, 0), new Quaternion());
		myGame.GetComponent<SpriteRenderer>().sortingOrder = 1000;

		yield return new WaitForSeconds(delay1);
		while (myGame.transform.position.x > 0)
		{
			myGame.transform.position = Vector3.MoveTowards(myGame.transform.position, new Vector3(0, 0), 50 * Time.deltaTime);
			yield return null;
		}
		yield return new WaitForSeconds(delay2);
		while (myGame.transform.position.x > -10)
		{
			myGame.transform.position = Vector3.MoveTowards(myGame.transform.position, new Vector3(-10, 0), 50 * Time.deltaTime);
			yield return null;
		}
	}
}
