using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class MenuCycler : MonoBehaviour
    {
        public bool Automatic;
        public string NextButton;
        public string PreviousButton;
        public MenuController Controller;
        public List<MenuBase> MenusToCycle = new List<MenuBase>();

        private void OnEnable()
        {
            if (Automatic)
            {
                MenusToCycle.Clear();
                MenusToCycle.AddRange(Controller.AllMenus);
            }
        }

        public void Next()
        {
            int index = MenusToCycle.IndexOf(Controller.CurrentMenu);
            if (index < 0)
                return;
            
            if (index + 1 < MenusToCycle.Count)
                index++;
            else
                index = 0;
            
            Controller.SwitchToMenu(MenusToCycle[index]);
        }

        public void Previous()
        {
            int index = MenusToCycle.IndexOf(Controller.CurrentMenu);
            if (index < 0)
                return;
            
            if (index - 1 >= 0)
                index--;
            else
                index = MenusToCycle.Count - 1;
            
            Controller.SwitchToMenu(MenusToCycle[index]);
        }

        void Update()
        {
            if (Controller.CurrentMenu != null)
            {
                if (!string.IsNullOrEmpty(NextButton) 
                && Input.GetButtonDown(NextButton))
                    Next();
                
                if (!string.IsNullOrEmpty(PreviousButton) 
                && Input.GetButtonDown(PreviousButton))
                    Previous();
            }
        }
    }
}