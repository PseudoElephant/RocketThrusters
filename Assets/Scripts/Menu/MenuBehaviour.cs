using System;
using UnityEngine;
using UnityEngine.UI;


public class MenuBehaviour : MonoBehaviour
{
        public Selectable focusedItem;

        protected void Start()
        {
            if (gameObject.activeSelf)
            {
                // HACK?
                focusedItem.interactable = false;
                focusedItem.interactable = true;
                
                focusedItem.Select();
                
           
            }
         
        }

        public void OnEnable()
        {
            focusedItem.Select();
        }
}
