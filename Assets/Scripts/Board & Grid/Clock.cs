using UnityEngine;

public class Clock : MonoBehaviour {
	//[SerializeField]
	public Sprite minNums;
	public Sprite hrNums;
	public Animator animator;

	private static Color selectedColor = new Color(.2f, .2f, .2f, 1.0f);
	public Transform visuals;
	public ClockType info;

	void Awake() {
		visuals = gameObject.transform.GetChild(0);
		animator = visuals.GetComponent<Animator>();
		InitClock();
	}

	public void Select() {
        // Debug.Log($"{gameObject.name}: selected!");
		visuals.GetComponent<SpriteRenderer>().color = selectedColor;
	}

	public void Deselect() {
        // Debug.Log($"{gameObject.name}: deselected!");
		visuals.GetComponent<SpriteRenderer>().color = Color.white;
	}

	public void ResetClock(){
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		InitClock();
	}

	public bool IsCompound(){
		if((info.hour > 0) && ((info.min > -1)|| info.gear)) return true;
		if((info.gear) && ((info.min > -1) || (info.hour > 0))) return true;
		if((info.min > -1) && ((info.hour > 0) || info.gear)) return true;
		return false;
	}
	
	public void PrintInfo(){
		Debug.Log($"[Debug]{this.name} info: {info.hour}:{info.min} Gear:[{info.gear}]");
	}

	private void InitClock(){
		info.hour = 0;
		info.min = -1;
		info.gear = false;

		int start = Random.Range(0, 3);

		switch(start){
			case 0:
			info.hour = Random.Range(1, 13);
			animator.SetInteger("Color", 1);
			break;
			case 1:
			info.min = Random.Range(0, 12) * 5;
			animator.SetInteger("Color", 2);
			break;
			default:
			info.gear = true;
			break;
		}
		UpdateVisuals();
		// Debug.Log($"FInished Init for clock {name}");
	}

	public void UpdateVisuals(){
		GameObject MinHand = visuals.GetChild(0).gameObject;
		GameObject HourHand = visuals.GetChild(1).gameObject;
		GameObject Gear = visuals.GetChild(2).gameObject;
		GameObject Face = visuals.GetChild(3).gameObject;

		if(info.min < 0 && info.hour < 1){
			Face.SetActive(false);
		}

		if(info.min < 0){
			MinHand.gameObject.SetActive(false);
		}else{
			MinHand.transform.rotation = Quaternion.identity;
			MinHand.SetActive(true);
			MinHand.transform.Rotate(0.0f, 0.0f, (info.min/5) * -30, Space.Self);
			if(!Face.activeSelf) Face.SetActive(true);
			Face.GetComponent<SpriteRenderer>().sprite = minNums;
			animator.SetInteger("Color", 2);
		}

		if(info.hour < 1){
			HourHand.SetActive(false);
		}else{
			HourHand.transform.rotation = Quaternion.identity;
			HourHand.SetActive(true);
			HourHand.transform.Rotate(0.0f, 0.0f, info.hour * -30, Space.Self);
			if(!Face.activeSelf) Face.SetActive(true);
			Face.GetComponent<SpriteRenderer>().sprite = hrNums;
			animator.SetInteger("Color", 1);
		}

		if(!info.gear){
			Gear.SetActive(false);
		}else{
			Gear.SetActive(true);
			animator.SetInteger("Color", 0);
		}

		if(IsCompound()){
			animator.SetInteger("Color", 3);
		}
	}

	public void MergeClocks(Clock other){
		if(other.info.min > -1){
			info.min = other.info.min;
		}
		if(other.info.hour > 0){
			info.hour = other.info.hour;
		}
		if(other.info.gear){
			info.gear = true;
		}
	
		animator.SetInteger("Color", 3);
		UpdateVisuals();
        if(other.info.min > 1 && other.info.hour > 0 && other.info.gear){
            Debug.Log("I should display my time");
        }
	}
}

public struct ClockType{
	public int min;
	public int hour;
	public bool gear;
}