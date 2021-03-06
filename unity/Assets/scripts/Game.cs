using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState { splash, menu, options, loading, playing, end };

public class Game : MonoBehaviour
{
	public Level levelLoader;
	public Level currentLevel;
	public TextBanner banner;
	public Title title;
	public TestStoryGenerator matchingGame;

	private GameState _currentState;
	private List<Tile> completedTiles;
	private List<string> completedWords;

	// Use this for initialization
	void Start ()
	{
		currentState = GameState.menu;
		matchingGame.finishCallback = AllTilesMatched;
		banner.endCallback = EndContinueClicked;
		title.endCallback = TitleContinueClicked;
	}
	
	public void TitleContinueClicked() {
		currentState = GameState.playing;
		Debug.Log ("Switching to playing");
	}

	private string ConvertTileToString(Tile t) {
		switch(t.name) {
		case "tile_boy": return "boy";
		case "tile_broken": return "brokenheart";
		case "tile_bulb": return "bulb";
		case "tile_cat": return "cat";
		case "tile_fire": return "fire";
		case "tile_girl": return "girl";
		case "tile_heart": return "heart";
		case "tile_nuke": return "nuke";
		case "tile_rainbow": return "rainbow";
		case "tile_world": return "world";
		}
		return "boy";
	}
	
	public void EndContinueClicked() {
		currentState = GameState.playing;
		Debug.Log ("Switching to playing");
	}
	
	public void AllTilesMatched(List<Tile> s) {
		currentState = GameState.end;
		List<string> stringies = new List<string>();
		foreach(Tile t in s) {
			stringies.Add (ConvertTileToString(t));
		}
		completedTiles = s;
		completedWords = stringies;
	}

	private void HideTitle() {
		Vector3 titlePosition = new Vector3(-25f, 2.5f, -1f);
		title.transform.position = titlePosition;
		title.visible = false;
	}
	
	private void HideBanner() {
		Vector3 bannerPosition = new Vector3(25f, 0f, -9.5f);
		banner.transform.position = bannerPosition;
		banner.visible = false;
		
	}
	
	private void ShowTitle() {
		Vector3 titlePosition = new Vector3(0f, 2.5f, -1f);
		title.transform.position = titlePosition;
		title.visible = true;
	}
	
	private void ShowBanner() {
		Vector3 bannerPosition = new Vector3(0f, 0f, -9.5f);
		banner.transform.position = bannerPosition;
		banner.visible = true;
	}

	private GameState currentState {
		get {
			return _currentState;
		}
		set {
			_currentState = value;
			switch(_currentState) {
			case GameState.splash:
				HideTitle();
				HideBanner();
				break;
				
			case GameState.menu:
				ShowTitle();
				HideBanner();
				break;
			case GameState.playing:
				matchingGame.load();
				HideTitle();
				HideBanner();
				break;
			case GameState.end:
				StoryGenerator gen = new StoryGenerator(new List<string>());
				banner.text = gen.story;
				HideTitle();
				ShowBanner();
				break;
			}
		}
	}

	// Update is called once per frame
	// input is taken here
	void Update ()
	{
		switch(currentState) {
		case GameState.splash:
			if(Input.GetKeyUp(KeyCode.Space)) {
				currentState = GameState.menu;
				Debug.Log ("Switching to menu");
			}
			break;
		case GameState.menu:
			if(Input.GetKeyUp(KeyCode.Space)) {
				currentState = GameState.playing;
				Debug.Log ("Switching to playing");
			}
			break;
		case GameState.playing:
			if(Input.GetKeyUp(KeyCode.Space)) {
				currentState = GameState.end;
				Debug.Log ("Switching to end");
			}
			break;
		case GameState.end:
			if(Input.GetKeyUp(KeyCode.Space)) {
				currentState = GameState.playing;
				Debug.Log ("Switching to playing");
			}
			break;
		}
	}

}

