/*
 * Copyright (c) 2017 Razeware LLC - Jeff Fisher
 */

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class BoardManager : MonoBehaviour {
	public static BoardManager instance;
	[SerializeField] private TextMeshProUGUI scoreBoard; 
	private Camera camera;

	[SerializeField] private Node _nodePrefab;
	[SerializeField] private Clock _clockPrefab;
	[SerializeField] private DisplayValue _display;
	public int xSize, ySize;

	private Node[,] nodes = null;
	public static Clock target;

	public bool IsShifting { get; set; }

	private float startX, startY;
	private Vector2 offset;
	private int score = -100;

	void Awake(){
		instance = this;
		camera = Camera.main;
		// bg = GetComponent<SpriteRenderer>();
		float aspectRatio = camera.aspect; //(width divided by height)
		float camSize = camera.orthographicSize; //The size value mentioned earlier
		float correctPosX = aspectRatio * camSize;

		offset = _nodePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size;
        startX = 0 - (correctPosX * 0.85f);
		startY = 0 - (camSize * 0.9f);

		target = Instantiate(_clockPrefab, new Vector3(startX + (offset.x * xSize * 1.25f), startY + (offset.y * ySize * 0.3f), 0), Quaternion.identity);
		target.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
		target.transform.parent = transform;
		target.name = $"Target Clock";
	}

	void Start () {
		SetTarget();
    }

	/**
	* Get a target time for the player 
	*/
	public void SetTarget() {
		score += 100;
		scoreBoard.text = $"{score}";
		List<int> mins = new List<int>();	
		List<int> hours = new List<int>();	
		List<bool> gears = new List<bool>();	

		do { // Do until we get a valid board
			// Debug.Log("In Loop!");
        	CreateBoard(startX, startY, offset.x, offset.y);

			ClockType temp;
			foreach (Node n in nodes) {
				if(n.clock != null){
					temp = n.clock.info;
					if(temp.min > 0) mins.Add(temp.min);
					if(temp.hour > 0) hours.Add(temp.hour);
					if(temp.gear) gears.Add(temp.gear);
				}
			}
		} while(mins.Count == 0 || hours.Count == 0 || gears.Count == 0);


		// Debug.Log($"[Debug] {mins.Count}");
		ClockType targetInfo = new ClockType();
		targetInfo.min = mins[Random.Range(0, mins.Count)];
		targetInfo.hour = hours[Random.Range(0, hours.Count)];
		targetInfo.gear = gears[Random.Range(0, gears.Count)];

		target.info = targetInfo;
		target.UpdateVisuals();
	}

	private void CreateBoard (float startX, float startY, float xOffset, float yOffset) {
		if(nodes != null) {
			Debug.Log($"nodes.length is {nodes.Length}");
			foreach (Node node in nodes) Destroy(node.gameObject); // Clear the nodes if there is anything in it.
		}
		nodes = new Node[xSize, ySize];

		int[] prevLeft = new int[ySize];
		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				Node newNode = Instantiate(_nodePrefab, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), _nodePrefab.transform.rotation);
				
				nodes[x, y] = newNode;
				newNode.transform.parent = transform;
				newNode.name = $"Tile({x},{y})";
			}
		}
    }

	public void OnEsc(){
		FindObjectOfType<AudioManager>().Play("Button");
		SceneManager.LoadScene("Start Menu");
	}

	public void OnTouch(InputAction.CallbackContext ctx){
		if(ctx.started) {
			Camera mainCam = Camera.main;
			Vector2 pointerPos = Mouse.current.position.ReadValue();
			pointerPos = mainCam.ScreenToWorldPoint(pointerPos);
			
			//Debug.Log("Raycast @ " + pointerPos);
			Collider2D[] hits = Physics2D.OverlapPointAll(pointerPos);
			foreach (var hit in hits) {
				// check if hit is a tile
				if (hit.gameObject.CompareTag("Tile")) {
					//Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
					hit.GetComponent<Node>().Touch();
				}
				//Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
				StartCoroutine(ScaleMe(hit.transform));
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
    * This method displays the a clock's time
    */
    public IEnumerator DisplayTime(Vector3 position, ClockType time){
        DisplayValue popup = Instantiate(_display, position, Quaternion.identity);
        popup.ShowTime(position, time);
		yield return new WaitForSeconds(1f);
    }

    public IEnumerator ScoreTime(Vector3 position, ClockType time){
        DisplayValue popup = Instantiate(_display, position, Quaternion.identity);
        popup.ScoreTime(position, time);
		yield return new WaitForSeconds(1f);
    }
}