/*
 * Copyright (c) 2017 Razeware LLC - Jeff Fisher
 */

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	public static BoardManager instance;
	private SpriteRenderer bg;
    public static GameObject displayInstance;

	public GameObject tile;
	public int xSize, ySize;

	private GameObject[,] nodes;

	public bool IsShifting { get; set; }

	void Start () {
		instance = this;
		bg = GetComponent<SpriteRenderer>();

		Vector2 offset = tile.GetComponentInChildren<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

	public void OnTouch(InputAction.CallbackContext ctx){
		if(ctx.started)
		{
			Camera mainCam = Camera.main;
			Vector2 pointerPos = Mouse.current.position.ReadValue();
			pointerPos = mainCam.ScreenToWorldPoint(pointerPos);
			
			Collider2D[] hits = Physics2D.OverlapPointAll(pointerPos);
			
			//Debug.Log("Raycast @ " + pointerPos);
			foreach (var hit in hits)
			{
				// check if hit is a tile
				if (hit.gameObject.CompareTag("Tile")){
					//Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
					hit.GetComponent<Node>().Touch();
				}
				//Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
				StartCoroutine(ScaleMe(hit.transform));
			}
		}
	}

	IEnumerator ScaleMe(Transform objTr) {
		objTr.localScale *= 1.1f;
		yield return new WaitForSeconds(0.1f);
		objTr.localScale /= 1.1f;
	}
	
    /**
    * This method Displays the ball's point value when a ball breaks a target
    */
    public void DisplayTime(Vector3 position, ClockType time){
        GameObject popup = Instantiate(displayInstance, position, Quaternion.identity);
        popup.GetComponent<DisplayValue>().ShowTime(position, time);
    }

	private void CreateBoard (float xOffset, float yOffset) {
		nodes = new GameObject[xSize, ySize];

        float startX = transform.position.x - (bg.bounds.extents.x * 0.75f);
		float startY = transform.position.y - (bg.bounds.extents.y * 0.75f);
		
		int[] prevLeft = new int[ySize];

		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				GameObject newClock = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
				
				nodes[x, y] = newClock;
				newClock.transform.parent = transform;
				newClock.name = "Tile(" + x + ", " + y + ")";

			}
		}
    }
}

public struct ClockType{
	public int min;
	public int hour;
	public bool gear;
}