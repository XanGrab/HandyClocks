using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 pos => transform.position;
	public GameObject clockPrefab;

	private Clock clock;
	private static Node previousSelected;

	private bool isSelected;

	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
	private bool matchFound;

    /**
     * Each Node object will make itself a default Clock obj
     */
    void Start(){
		clockPrefab = Instantiate(clockPrefab, pos, Quaternion.identity);
		clockPrefab.name = gameObject.name + " Clock";
		clockPrefab.transform.parent = transform;
        clock = clockPrefab.GetComponent<Clock>(); 
    }

	private void Select() {
		isSelected = true;
        if(clock) clock.GetComponent<Clock>().Select();
		previousSelected = this;
	}

	private void Deselect() {
		isSelected = false;
        if(clock) clock.GetComponent<Clock>().Deselect();
		previousSelected = null;
	}

	public void Touch()
	{ 
		Debug.Log("Tile" + gameObject.transform.position + " touched!");
		if(BoardManager.instance.IsShifting){
			return;
		}

		if(isSelected){
			Deselect();
		}else{
			if(previousSelected == null){ 
				Select();
			}else{
				if (GetAllAdjacentTiles().Contains(previousSelected.gameObject)) {
                    if(previousSelected.clock){
					    SwapOrMergeClock(previousSelected.clock);
                    }
					previousSelected.Deselect();
				} else {
					previousSelected.Deselect();
					Select();
				}
			}
		}
	}

	/**
	 * This method is utilized when swapping tiles to ensure the swapping tiles are adjacent
	 */
	public GameObject GetAdjacent(Vector2 castDir) {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
		if (hit.collider != null) {
			return hit.collider.gameObject;
		}
		return null;
	}

	private List<GameObject> GetAllAdjacentTiles() {
		List<GameObject> adjacentTiles = new List<GameObject>();
		for (int i = 0; i < adjacentDirections.Length; i++) {
			adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
		}
		return adjacentTiles;
	}
	
	public void SwapOrMergeClock(Clock other) {
		bool merge = true;
		if(other.info.min > 1 && clock.info.min > 1){
			merge = false;
		}
		if(other.info.hour > 0 && clock.info.hour > 0){
			merge = false;
		}
		if(other.info.gear && clock.info.gear){
			merge = false;
		}

		if(merge){
			clock.mergeClocks(other);
		    Destroy(other.gameObject);
		}else{
            SwapClock(other);
		}
		//TODO: Add SFX
	}	

	public void SwapClock(Clock other) {
		Node otherNode = other.transform.parent.GetComponent<Node>();

        Vector3 tempPos = other.transform.position;
        other.transform.position = clock.transform.position;
        clock.transform.position = tempPos;

        // Update in Hierarchy
        Transform parent = other.transform.parent;
		other.transform.parent = clockPrefab.transform.parent;
		other.transform.parent = parent;
		
		Clock tempClock = other;
		otherNode.clock = this.clock;
		this.clock = tempClock;

        // TODO: Check this logic
		GameObject temp = otherNode.clockPrefab;
		otherNode.clockPrefab = this.clockPrefab;
		this.clockPrefab = temp;

		clockPrefab.name = gameObject.transform.parent.name + " Clock";
		other.name = other.transform.parent.name + " Clock";
	}
	
}
