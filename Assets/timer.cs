using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {

	public Renderer rend;
	public ProceduralMaterial substance;
	public Button butS;
	public Button butM;
	public Text status;

	//int timerCounter;


	// Use this for initialization
	void Start () {
		butS = GameObject.Find("ButtonSnow").GetComponent<Button>();
		butM = GameObject.Find("ButtonMelt").GetComponent<Button>();
		status = GameObject.Find ("TextStatus").GetComponent<Text> ();

		butS.onClick.AddListener (() => snowClick());
		butM.onClick.AddListener (() => meltClick());
		rend = GetComponent<Renderer>();
		substance = rend.sharedMaterial as ProceduralMaterial;
		substance.cacheSize = ProceduralCacheSize.NoLimit;

		//timerCounter = 1;
	}

	// Update is called once per frame
	void Update () {
		if (status.text == "snow")
			increaseSnow (0.001f);
		else if (status.text == "melt")
			decreaseSnow (0.001f);
	}

	void increaseSnow (float f) {
		
		float oldVal = substance.GetProceduralFloat ("Fresh Snow");
		print(oldVal);
		if (!substance.isProcessing) {
			substance.SetProceduralFloat ("Fresh Snow", oldVal + f);
			substance.RebuildTextures();
		}
	}

	void decreaseSnow (float f) {
		butS.interactable = false;

		float oldVal = substance.GetProceduralFloat ("Melted Snow");
		float oldPud = substance.GetProceduralFloat ("Puddles_Position");
		print(oldVal);
		if (!substance.isProcessing) {
			if (oldVal == 1f) {
				status.text = "pause";
				substance.SetProceduralFloat ("Fresh Snow", 0);
				substance.SetProceduralFloat ("Melted Snow", 0);
				substance.SetProceduralFloat ("Puddles_Position", 0);
				substance.RebuildTextures ();
				butS.interactable = true;
			} else {
				substance.SetProceduralFloat ("Melted Snow", oldVal + f);
				if (oldVal < 0.6f)
					substance.SetProceduralFloat ("Puddles_Position", oldPud + (f/2));
				else 
					substance.SetProceduralFloat ("Puddles_Position", oldPud - (f));
				substance.RebuildTextures ();
			}
		}
	}


		
	void snowClick(){
		status.text = "snow";
	}

	void meltClick(){
		status.text = "melt";
	}
}
