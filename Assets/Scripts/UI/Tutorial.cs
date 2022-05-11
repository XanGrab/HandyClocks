using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : MonoBehaviour {
	public static Tutorial instance;
	private Camera camera;

	[SerializeField] private Node _nodePrefab;
	[SerializeField] private Clock _clockPrefab;
	[SerializeField] private DisplayValue _display;
	public int xSize, ySize;

	private Node[,] nodes = null;

	// public bool IsShifting { get; set; }

	private float startX, startY;
	private Vector2 offset;

	void Awake(){
		instance = this;
		camera = Camera.main;
		// bg = GetComponent<SpriteRenderer>();
		float aspectRatio = camera.aspect; //(width divided by height)
		float camSize = camera.orthographicSize; //The size value mentioned earlier
		float correctPosX = aspectRatio * camSize;

		offset = _nodePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size;
        startX = 0 - (correctPosX * 0.78f);
		startY = 0 - (camSize * 0.9f);
	}

	void Start () { CreateBoard(startX, startY, offset.x, offset.y); }

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

	public void OnEsc(){ SceneManager.LoadScene("Start Menu"); }

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