/*
 * Copyright (c) 2017 Razeware LLC - Jeff Fisher
 */

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BoardManager : MonoBehaviour {
	public static BoardManager instance;
	private SpriteRenderer bg;
    public static GameObject displayInstance;

	[SerializeField] private Node _nodePrefab;
	[SerializeField] private Clock _clockPrefab;
	public int xSize, ySize;

	private Node[,] nodes;
	private Clock target;

	public bool IsShifting { get; set; }

	void Start () {
		instance = this;
		bg = GetComponent<SpriteRenderer>();

		Vector2 offset = _nodePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

	private void CreateBoard (float xOffset, float yOffset) {
		nodes = new Node[xSize, ySize];

        float startX = transform.position.x - (bg.bounds.extents.x * 0.75f);
		float startY = transform.position.y - (bg.bounds.extents.y * 0.75f);
		
		int[] prevLeft = new int[ySize];

		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				Node newNode = Instantiate(_nodePrefab, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), _nodePrefab.transform.rotation);
				
				nodes[x, y] = newNode;
				newNode.transform.parent = transform;
				newNode.name = $"Tile({x},{y})";
			}
		}

		target = Instantiate(_clockPrefab, new Vector3(startX + (xOffset * xSize * 1.25f), startY + (yOffset * ySize * 0.4f), 0), Quaternion.identity);
		target.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
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
				// StartCoroutine(ScaleMe(hit.transform));
			}
		}
	}

    /**
    * This method displays the a point value
    */
	public static IEnumerator ScaleMe(Transform objTr) {
		objTr.localScale *= 1.1f;
		yield return new WaitForSeconds(0.1f);
		objTr.localScale /= 1.1f;
	}
	
    /**
    * This method displays the a point value
    */
    public void DisplayTime(Vector3 position, ClockType time){
        GameObject popup = Instantiate(displayInstance, position, Quaternion.identity);
        popup.GetComponent<DisplayValue>().ShowTime(position, time);
    }

	/**
	*  
	*/
	public void SetTarget(){
		
	}
}