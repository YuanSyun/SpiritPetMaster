﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpiritPetMaster
{
    public class NewPetPanel : MonoBehaviour
    {
        public string PetKind = "";
        public Image PetImage;
        public Text PetNameInput;
        public GameObject PetViewPrefab;

        public void SetPetImage()
        {
            var sprite = Resources.Load(PetKind+"/Idle/idle_1", typeof(Sprite));
            if (sprite != null)
            {
                PetImage.sprite = (Sprite)sprite;
            }
        }

        public string GetNewPetName()
        {
            return PetNameInput.text;
        }

        public void NewPetNaming(string _petkind)
        {
            PetKind = _petkind;
            NewPetNaming();
        }
        public void NewPetNaming()
        {
            SetPetImage();
            gameObject.SetActive(true);
        }

        public void AddNewPet()
        {
            PetViewController.instance.NewPetView(PetKind, PetNameInput.text);
            PetViewController.instance.FocusPetView();
            // PetViewController.instance.FreePetView();
        }
        public void AddNewPetForStage()
        {
            int id;
            do
            {
                id = Random.Range(0, PetViewController.PET_ID_RANGER);
            } while (PlayerData.instance.CheckIDExisted(id));
            GameObject _new_pet_view = Instantiate(PetViewPrefab, Vector3.zero, Quaternion.identity, transform);
            PetView _pet_view = _new_pet_view.GetComponent<PetView>();
            _pet_view.NewPet(id, PetKind, GetNewPetName());
            _new_pet_view.GetComponent<SpriteRenderer>().enabled = false;

            PlayerData.instance.SavePlayerData();
        }

    }
}


