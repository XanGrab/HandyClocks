// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerEnterHandler {
    public Vector2 pos => transform.position;
	public GameObject clockPrefab;

	private Clock clock;
	private static Node previousSelected;

	private bool isSelected;

	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
	// private bool matchFound;

    /**
     * Each Node object will make itself a default Clock obj
     */
    void Start(){
		clockPrefab = Instantiate(clockPrefab, pos, Quaternion.identity);
		clockPrefab.name = $"{gameObject.name}'s Clock";
		clockPrefab.transform.parent = transform;
        clock = clockPrefab.GetComponent<Clock>(); 
    }

	private void Select() {
        Debug.Log($"{gameObject.name}: selected!");
        isSelected = true;
        clock.GetComponent<Clock>().Select();
        Reporter.ReportSelect(clock);
        previousSelected = this;
	}

	private void Deselect() {
        Debug.Log($"{gameObject.name}: deselected!");
        isSelected = false;
        clock.GetComponent<Clock>().Deselect();
        Reporter.ReportDeselect(clock);
        previousSelected = null;
	}

	public void OnPointerEnter(PointerEventData data) {
		Debug.Log($"{name} entered!");
		// StartCoroutine(BoardManager.ScaleMe(transform));
	}

	public void Touch()	{ 
		if(BoardManager.instance.IsShifting) return;

        if(clock){
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
        }else{
            Debug.Log(gameObject.name + ": I don't have a clock!");
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
			clock.mergeClocks(other);
		    Destroy(other.gameObject);
		}else{
			Debug.Log($"[Debug] Swap: {this.clock} & {other}");
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
        // Transform parent = other.transform.parent;
		// other.transform.parent = clockPrefab.transform.parent;
		// other.transform.parent = parent;
		
		// Clock temp = other;
		// otherNode.clock = this.clock;
		// this.clock = temp;

		// otherNode.clock.name = $"{otherNode.name}'s Clock";
		// this.clock.name = $"{name}'s Clock";
		otherNode.clock.Deselect();
		this.clock.Deselect();
	}	
}
