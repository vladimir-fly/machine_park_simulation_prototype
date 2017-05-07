using System;
using UnityEngine;
using UnityEngine.UI;

namespace MPSPrototype
{
    public class MainScreen : MonoBehaviour
    {
        public Slider MachinesNumber;
        public Slider MaxWorkingTime;
        public Slider MaxRestorationTime;

        public Dropdown ApproximationPoints;
        public Button StartButton;

        public Text MachinesNumberLabel;
        public Text ApproximationPointsLabel;
        public Text MaxWorkingTimeLabel;
        public Text MaxRestorationTimeLabel;
        public Text AverageWorkingTimeLabel;
        public Text AverageRestorationTimeLabel;

        private int[] _approximationPointsNumber = {10, 100, 1000};

        private void Start()
        {
            if (!CheckBindings()) return;
            SetValuesChangedListeners();
            MachinesNumber.onValueChanged.Invoke(MachinesNumber.value);
            MaxWorkingTime.onValueChanged.Invoke(MaxWorkingTime.value);
            MaxRestorationTime.onValueChanged.Invoke(MaxRestorationTime.value);
        }

        public void Initialize(Action<SimulationInitData, Action<float, float>> initSimulation,
                               Action startSimulation,
                               Action<float> initCameraSize)
        {
            if (!CheckParams(initSimulation, startSimulation, initCameraSize) || !CheckBindings()) return;

            SetValuesChangedListeners();
            StartButton.onClick.AddListener(() =>
                {
                    initSimulation(
                        new SimulationInitData
                            {
                                MachinesNumberValue = (int) MachinesNumber.value,
                                MaxWorkingTimeValue = (int) MaxWorkingTime.value,
                                MaxRestorationTimeValue = (int) MaxRestorationTime.value,
                                ApproximationPointsNumber = _approximationPointsNumber[ApproximationPoints.value]
                            },
                        (summaryWorkingTime, summaryRepairingTime) =>
                        {
                            AverageWorkingTimeLabel.text =
                                string.Format("Average working time: {0}.",
                                    summaryWorkingTime / MachinesNumber.value);
                            AverageRestorationTimeLabel.text =
                                string.Format("Average restoration time: {0}.",
                                    summaryRepairingTime / MachinesNumber.value);
                        });
                    initCameraSize(MachinesNumber.value);
                    startSimulation();
                });
        }

        private void SetValuesChangedListeners()
        {
            MachinesNumber.onValueChanged.AddListener(
                value => MachinesNumberLabel.text = string.Format("Machines number: {0}", value));

            MaxWorkingTime.onValueChanged.AddListener(
                value => MaxWorkingTimeLabel.text = string.Format("Max working time: {0}", value));

            MaxRestorationTime.onValueChanged.AddListener(
                value => MaxRestorationTimeLabel.text = string.Format("Max restoration time: {0}", value));
        }

        private static bool CheckParams(Action<SimulationInitData, Action<float, float>> init,
                                        Action startSimulation,
                                        Action<float> initCameraSize)
        {
            if (init == null) Debug.LogError("[MainScreen][CheckParams] Initialize callback not setted!");
            if (startSimulation == null) Debug.LogError("[MainScreen][CheckParams] Start simulation callback not setted!");
            if (initCameraSize == null) Debug.LogError("[MainScreen][CheckParams] Initialize camera callback not setted!");

            return init != null && startSimulation != null && initCameraSize != null;
        }

        private bool CheckBindings()
        {
            if (MachinesNumber == null) Debug.LogError("[MainScreen][CheckBindings] Machines number slider not assigned!");
            if (MaxWorkingTime == null)Debug.LogError("[MainScreen][CheckBindings] Max working time slider not assigned!");
            if (MaxRestorationTime == null) Debug.LogError("[MainScreen][CheckBindings] Max restoration time slider not assigned!");
            if (ApproximationPoints == null) Debug.LogError("[MainScreen][CheckBindings] Approximation points dropdown menu not assigned!");
            if (StartButton == null) Debug.LogError("[MainScreen][CheckBindings] Start button not assigned!");
            if (MachinesNumberLabel == null) Debug.LogError("[MainScreen][CheckBindings] Machines number label not assigned!");
            if (MaxWorkingTimeLabel == null) Debug.LogError("[MainScreen][CheckBindings] Max working time label not assigned!");
            if (MaxRestorationTimeLabel == null) Debug.LogError("[MainScreen][CheckBindings] Max restoration time label not assigned!");
            if (AverageWorkingTimeLabel == null) Debug.LogError("[MainScreen][CheckBindings] Average working time label not assigned!");
            if (AverageRestorationTimeLabel == null) Debug.LogError("[MainScreen][CheckBindings] Average restoration time label not assigned!");

            return MachinesNumber != null && MaxWorkingTime != null &&
                   MaxRestorationTime != null && ApproximationPoints != null &&
                   StartButton != null && MachinesNumberLabel != null &&
                   MaxWorkingTimeLabel != null && MaxRestorationTimeLabel != null &&
                   AverageWorkingTimeLabel != null && AverageRestorationTimeLabel != null;
        }
    }
}