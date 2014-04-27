﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// this class is used to test the level loader
public class TestStoryGenerator : MonoBehaviour {
	public GameObject levelCanvas;
	public GameObject tilePrefab;
	public GameObject tileStartPoint;
	public Sprite[] sprites;

	public int tilesUpfacing = 0;
	public static List<Tile> upTiles = new List<Tile> ();
	public static int combinedTiles = 0;
	public static List<Tile> storedTiles = new List<Tile>();

	float nextUsage;
	float delay = 1f;
	bool killed = false;
	private MyFunction f;

	// Use this for initialization
	void Start () {

	}

	public void load(){
		LoadWordMap loader = new LoadWordMap ("levels", 0, tilePrefab, levelCanvas, tileStartPoint,
		                                      sprites);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.F5))
			load ();
		if(!killed)
			nextUsage = Time.time + delay;
		if(upTiles != null)
			if (upTiles.Count == 2)
				checkSameTileFaces();
	}

	public void checkSameTileFaces(){
		if (!killed)
			killed = true;

		if (Time.time > nextUsage && killed) {
			killed = false;
			if (upTiles [0].tileName == upTiles [1].tileName) {
				upTiles [1].move ();
				storedTiles.Add(upTiles[1]);
				upTiles [0].kill ();
				upTiles.RemoveRange (0, 2);
				combinedTiles++;
			} else {
				upTiles [0].switchBack ();
				upTiles [1].switchBack ();
				upTiles.RemoveRange (0, 2);
			}
		}

		if (storedTiles.Count == 6)
			f (storedTiles);
	}

	public MyFunction finishCallback{
		set{
			f = value;
		}
	}
}

public delegate void MyFunction (List<Tile> s);