using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerEnterHandler {
    public Vector2 pos => transform.position;
	[SerializeField] private Clock _clockPrefab;

	public Clock clock;
	private static Node previousSelected;

	private bool isSelected;
	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    /**
     * Each Node object will make itself a default Clock obj
     */
    private void Awake() {
		clock = Instantiate(_clockPrefab, pos, Quaternion.identity);
		clock.name = $"{gameObject.name}'s Clock";
		clock.transform.parent = transform;
	}

	private void Select() {
		clock.PrintInfo();
        isSelected = true;
        clock.Select();
        Reporter.ReportSelect(clock);
        previousSelected = this;
	}

	private void Deselect() {
        isSelected = false;
        clock.Deselect();
        Reporter.ReportDeselect(clock);
        previousSelected = null;
	}

	public void OnPointerEnter(PointerEventData data) {
		Debug.Log($"{name} entered!");
	}

	public void Touch()	{ 
		if(BoardManager.instance.IsShifting) return;

		if(!clock){
            Debug.Log($"{gameObject.name} : I don't have a clock!");
            if(previousSelected != null && previousSelected.clock.IsCompound()){
				BreakClock(previousSelected);
				previousSelected.Deselect();
			} 
			return;
		}

		if(isSelected){
			Deselect();
		}else{
			if(previousSelected == null){ 
				if (clock.info.Equals(BoardManager.target.info)){
					StartCoroutine(Score());
				}
	
				Select();
			}else{
				if (GetAllAdjacentTiles().Contains(previousSelected.gameObject)) {
					if(previousSelected.clock)
						SwapOrMergeClock(previousSelected.clock);
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
		if (other.info.min > -1 && clock.info.min > -1){
			merge = false;
		}
		if (other.info.hour > 0 && clock.info.hour > 0){
			merge = false;
		}
		if (other.info.gear && clock.info.gear){
			merge = false;
		}

		if(merge){
			Debug.Log($"[Debug] Merge: {this.clock} & {other}");
            Reporter.ReportMerge(clock);
			clock.MergeClocks(other);
		    Destroy(other.gameObject);
		}else{
			Debug.Log($"[Debug] Swap: {this.clock} & {other}");
            SwapClock(other);
		}
		//TODO: Add SFX
	}	

	public void SwapClock(Clock other) {
		Node otherNode = other.transform.parent.GetComponent<Node>();

		otherNode.clock.transform.position = this.pos;
		this.clock.transform.position = otherNode.pos;

        // Update in Hierarchy
		otherNode.clock.transform.SetParent(transform);
		this.clock.transform.SetParent(otherNode.transform);

		Clock temp = this.clock;
		this.clock = otherNode.clock;
		otherNode.clock = temp;

		// otherNode.clock.Deselect();
		this.clock.Deselect();

		otherNode.clock.name = $"{otherNode.name}'s Clock";
		this.clock.name = $"{this}'s Clock";
	}

	/**
	* Unmerge a clock back on to the `other` Node
	*/
	public void BreakClock(Node other){
		// Debug.Log($"Break clock! empty");
		// Make a new clock
		clock = Instantiate(_clockPrefab, pos, Quaternion.identity);
		clock.name = $"{name}'s Clock";
		clock.transform.parent = transform;
		// clock.PrintInfo();

		// Cut and paste a field from other's clock info
		ClockType newInfo = new ClockType();
		if(other.clock.info.min > -1){
			newInfo.min = other.clock.info.min;
			other.clock.info.min = -1;
		}else{
			newInfo.hour = other.clock.info.hour;
			other.clock.info.hour = 0;
			newInfo.min = -1;
		}
		clock.info = newInfo;
		// clock.PrintInfo();

		clock.UpdateVisuals();
		other.clock.UpdateVisuals();
		SwapClock(other.clock);
	}

	IEnumerator Score(){
		yield return StartCoroutine(BoardManager.instance.DisplayTime(pos, clock.info));
		BoardManager.instance.SetTarget();
	}
}
