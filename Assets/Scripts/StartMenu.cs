﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpiritPetMaster{
	public class StartMenu : MonoBehaviour {

		public bool nameUsed = false;
		public GameObject sameNameWarning;
		public GameObject petsContainer;
		public GameObject startMenu;

		public void isUsed (Text _name){
			PlayerData.instance.PlayerName = _name.text;
			string[] _petids = PlayerData.instance.GetPetsId();
			if(_petids==null)
				nameUsed = false;
			else
				nameUsed = true;
		}

		public void toNameWarning(){
			if(nameUsed){
				sameNameWarning.SetActive(true);
			}
			else{
				reloadUser();
				startMenu.SetActive(false);
			}
		}

		public void rebuildUser(string _name){
			PlayerData.instance.PlayerName = _name;
			rebuildUser();
		}
		public void rebuildUser(){
			PlayerData.instance.ClearPlayerData();
			reloadUser();
		}
		public void reloadUser(string _name){
			PlayerData.instance.PlayerName = _name;
			reloadUser();
		}
		public void reloadUser(){
			petsContainer.GetComponent<PetViewController>().ClearPetsInScene();
			petsContainer.GetComponent<PetViewController>().UpdatePetView();
		}

		

	}
}