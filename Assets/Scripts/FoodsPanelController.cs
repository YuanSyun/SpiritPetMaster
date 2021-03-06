﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SpiritPetMaster
{
	public class FoodsPanelController : MonoBehaviour {
        public GameObject foodController;
		public List<Text> foodsCounterText;

        void Update()
        {
            for(int i=0;i<FoodController.foodsNumber;++i){
                if(foodsCounterText[i]!=null)
                    foodsCounterText[i].text = "× "+ PlayerData.instance.foodsCounter[i].ToString();
            }
        }

        public void FoodButton(int ind){
            if(PlayerData.instance.foodsCounter[ind]>0)
            {
                foodController.GetComponent<FoodController>().InstanceFood(ind);
                PlayerData.instance.foodsCounter[ind] -= 1;
            }
        }
	}
}