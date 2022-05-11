using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
// using System.Collections.Generic;
using UnityEngine.SceneManagement;
// using TMPro;

public class Tutorial : MonoBehaviour {
	public static Tutorial instance;
	private Camera camera;

	[SerializeField] private TutNode _nodePrefab;
	[SerializeField] private Clock _clockPrefab;
	[SerializeField] private DisplayValue _display;

	private TutNode[,] nodes = null;

	private int xSize, ySize;
	private float startX, startY;
	private Vector2 offset;

	void Awake(){
		instance = this;
		camera = Camera.main;
		float aspectRatio = camera.aspect; //(width divided by height)
		float camSize = camera.orthographicSize; //The size value mentioned earlier
		float correctPosX = aspectRatio * camSize;

		offset = _nodePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size;
        xSize = 3;
        ySize = 3;
        startX = 0 - (correctPosX * 0.75f);
		startY = 0 - (camSize * 0.75f);
	}

	void Start () { CreateBoard(startX, startY, offset.x, offset.y); }

	private void CreateBoard (float startX, float startY, float xOffset, float yOffset) {
		nodes = new TutNode[xSize, ySize];

        ClockType[,] preset = new ClockType[xSize, ySize];
        // Top
        preset[0,2].gear = true;
        preset[0,2].min = -1;
        preset[1,2].min = 0;
        preset[2,2].hour = 3;
        preset[2,2].min = -1;
        // Bottom
        preset[0,0].hour = 9;
        preset[0,0].min = -1;
        preset[1,0].hour = 12;
        preset[1,0].min = -1;
        preset[2,0].hour = 3;
        preset[2,0].min = -1;

		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
                if (y == 1) continue;

				TutNode newNode = Instantiate(_nodePrefab, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), _nodePrefab.transform.rotation);
				nodes[x, y] = newNode;
				newNode.transform.parent = transform;
				newNode.name = $"Tile({x},{y})";
                
                // Assign the presets
                newNode.clock.info = preset[x, y];
                newNode.clock.UpdateVisuals();
			}
		}

    }

	public void OnEsc() {
        try {
            FindObjectOfType<AudioManager>().Play("Button");
        } catch (Exception e) {};
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
					hit.GetComponent<TutNode>().Touch();
				}
				//Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
				StartCoroutine(ScaleMe(hit.transform));
			}
		}
	}

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
}