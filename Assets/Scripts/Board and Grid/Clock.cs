/*
 * Copyright (c) 2017 Razeware LLC - Jeff Fisher
 */

//using System;
using UnityEngine;
// using UnityEngine.InputSystem;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine.UI;

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
	}

	public void Start()
	{
		initClock();
	}

	public void Select() {
		visuals.GetComponent<SpriteRenderer>().color = selectedColor;
	}

	public void Deselect() {
		visuals.GetComponent<SpriteRenderer>().color = Color.white;
	}

	private void initClock(){
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
			case 2:
			info.gear = true;
			break;
		}
		UpdateVisuals();
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
			MinHand.SetActive(true);
			MinHand.transform.Rotate(0.0f, 0.0f, info.min * 30.0f, Space.Self);
			if(!Face.activeSelf) Face.SetActive(true);
			Face.GetComponent<SpriteRenderer>().sprite = minNums;
		}

		if(info.hour < 1){
			HourHand.SetActive(false);
		}else{
			HourHand.SetActive(true);
			HourHand.transform.Rotate(0.0f, 0.0f,info.hour * 30.0f, Space.Self);
			if(!Face.activeSelf) Face.SetActive(true);
			Face.GetComponent<SpriteRenderer>().sprite = hrNums;
		}

		if(!info.gear){
			Gear.SetActive(false);
		}else{
			Gear.SetActive(true);
		}
	}

	public void mergeClocks(Clock other){
		if(other.info.min > 1){
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
            //TODO: Diaply Time
            Debug.Log("I should display my time");
            // BoardManager.DisplayTime(this.transform.position, info);
        }
		//StartCoroutine(HandyBoardManager.instance.FindNullTiles());
	}

	public void resetClock(){
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		initClock();
	}

}