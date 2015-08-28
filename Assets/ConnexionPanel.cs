using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnexionPanel : MonoBehaviour {

	public Image	color;
	public Text 	username;
	public Text 	errorText;
	public Slider 	redSlider;
	public Slider 	blueSlider;
	public Slider 	greenSlider;
	public Button	connectButton;
	public GameManager game;

	void Start()
	{
		connectButton.onClick.AddListener(() => game.Connect());
	}

	void Update()
	{
		Color newColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);
		color.GetComponent<Image>().color = newColor;
	}
}
