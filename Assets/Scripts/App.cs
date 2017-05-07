using UnityEngine;

namespace MPSPrototype
{
    public class App : MonoBehaviour
    {
        public MainScreen MainScreen;
        public MachinePark MachinePark;
        public Camera Camera;

        private void Start()
        {
            Debug.Log("<color=green>[App]</color>" +
                      "<color=blue>[Start]</color>" +
                      "<b><color=#003653> Welcome!</color></b>");

            if (!CheckBindings()) return;

            MainScreen.Initialize(
                MachinePark.Initialize,
                MachinePark.StartWork,
                (size) => Camera.orthographicSize =  Mathf.Sqrt(size) * 1.3f);
        }

        private bool CheckBindings()
        {
            if (MainScreen == null) Debug.LogError("[App][CheckBindings] MainScreen not assigned!");
            if (MachinePark == null) Debug.LogError("[App][CheckBindings] MachinePark not assigned!");
            if (Camera == null) Debug.LogError("[App][CheckBindings] Camera not assigned!");

            return MainScreen != null && MachinePark != null && Camera != null;
        }
    }
}