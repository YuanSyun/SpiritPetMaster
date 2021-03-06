﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiritPetMaster
{
    public class FoodController : MonoBehaviour
    {

        public const int foodsNumber = 6;
        public float FoodHeight;
        public List<GameObject> FoodList;

        //public Vector2 FoodSize = new Vector2(150,150);
        
        public void InstanceFood(int _index)
        {
            if((0 <= _index) && (_index < FoodList.Count))
            {
                Vector2 pos = Camera.main.transform.position;
                pos.y += FoodHeight;

                Instantiate(FoodList[_index], pos, Quaternion.identity, transform);
            }
        }
        public void InstanceFood(GameObject _food)
        {
                Vector2 pos = Camera.main.transform.position;
                pos.y += FoodHeight;

                Instantiate(_food, pos, Quaternion.identity, transform);
        }
    }
    
}


