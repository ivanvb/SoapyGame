using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.UI;


public class ResourcesController : MonoBehaviour {

	public Canvas canvas;
	public TextMeshProUGUI textMeshProUGUI;
	public Slider slider;

	Level currentLevel;
	List<KeyValuePair<string, int>> ingredients;
	List<TextMeshProUGUI> ingredients_count;
	Dictionary<TextMeshProUGUI, Slider> sliders;

	// Use this for initialization
	void Start () {
		ingredients_count = new List<TextMeshProUGUI>();
		GetComponent<LevelLoader>().onLevelLoaded += LoadLevel;

		GameObject.Find("Scripter").GetComponent<CardMatchController>().onMatch += UpdateIng;

		sliders = new Dictionary<TextMeshProUGUI, Slider>();
	}

	/* Sets the current level and start loading thingredient_counter */
	private void LoadLevel(Level level)
	{
		currentLevel = level;
		ingredients = GetIngredients();
		InitializeComponents();
	}

	/* Returns a list of Pairs with each ingredient as well as the number of times the user is required to find them */
	private List<KeyValuePair<string, int>> GetIngredients()
	{
		List<KeyValuePair<string, int>> ingredients = new List<KeyValuePair<string, int>>();

		foreach(string ingredient in currentLevel.ingredients)
		{
			Regex regexQuantity = new Regex(@"(\d)+");
			Match matchQuantity = regexQuantity.Match(ingredient);
			
			Regex regexIngredient = new Regex(@"([a-zA-Z])+");
			Match matchIngredient = regexIngredient.Match(ingredient);

			KeyValuePair<string, int> currentIngredient = new KeyValuePair<string, int>
																(matchIngredient.ToString(), Convert.ToInt32(matchQuantity.ToString()));

			ingredients.Add(currentIngredient);
		}
		return ingredients;
	}

	private void InitializeComponents() //Needs refactoring
	{
		textMeshProUGUI.rectTransform.localScale = new Vector3(PhysicsConstants.ONE_CANVAS_SCALE, 
																PhysicsConstants.ONE_CANVAS_SCALE,
																PhysicsConstants.ONE_CANVAS_SCALE);

		slider.transform.localScale = new Vector3(PhysicsConstants.ONE_CANVAS_SCALE,
												PhysicsConstants.ONE_CANVAS_SCALE,
												PhysicsConstants.ONE_CANVAS_SCALE);

		float n = PhysicsConstants.CANVAS_OFFSET;

		foreach(KeyValuePair<string, int> ingredient in ingredients)
		{
			TextMeshProUGUI currentText = textMeshProUGUI;
			currentText.text = ingredient.Key;
			currentText = Instantiate(currentText, new Vector3(-1.2f, 5.05f - n / 196f), new Quaternion());
			currentText.transform.SetParent(canvas.transform);

			Slider currentSlider = slider;
			currentSlider = Instantiate(currentSlider, new Vector3(-(1 / 2 - 0.05f), 5f - n / 196f), new Quaternion());

			currentSlider.maxValue = ingredient.Value;
			currentSlider.minValue = 0;
			currentSlider.value = 0;
			currentSlider.transform.SetParent(canvas.transform);

			TextMeshProUGUI ingredient_counter = textMeshProUGUI;
			ingredient_counter = Instantiate(ingredient_counter, new Vector3(2.4f, 5.05f - n / 196f), new Quaternion());
			ingredient_counter.text = "0/" + ingredient.Value.ToString();
			ingredient_counter.transform.SetParent(canvas.transform);
			ingredient_counter.name = ingredient.Key + "score_text";

			sliders[ingredient_counter] = currentSlider;
			
			ingredients_count.Add(ingredient_counter);

			n += 100f;
		}
	}

	

	/* Adds one point to the ingredient counter */

	private void UpdateIng(string ing)
	{
		foreach(TextMeshProUGUI text in ingredients_count)
		{
			if(text.name.Contains(ing))
			{
				Regex totalRegex = new Regex(@"(?<=\/)(\d)+");
				Match matchTotal = totalRegex.Match(text.text);

				Regex regex = new Regex(@"(\d)+(?=\/)");
				Match match = regex.Match(text.text);
				
				int current_score = Convert.ToInt32(match.ToString());

				if (current_score < Convert.ToInt32(matchTotal.ToString())) 
				{

					current_score++;
					text.text = text.text.Substring(match.ToString().Length);
					text.text = text.text.Insert(0, current_score.ToString());

					StartCoroutine("fillBar", sliders[text]);
				}

			}
		}
	}

	/* Adds one point to the value of a slider */
	private IEnumerator fillBar(Slider slider)
	{
		float value = slider.value + 1;
		for(float i = slider.value; i <= value; i += 0.018f)
		{
			slider.value = i;
			yield return null;
		}
	}

}

