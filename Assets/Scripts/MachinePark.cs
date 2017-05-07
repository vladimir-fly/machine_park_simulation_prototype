using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace MPSPrototype
{
    public class MachinePark : MonoBehaviour
    {
        public AnimationCurve FailureIntensityCurve;
        private List<GameObject> _machines;

        public void Initialize(SimulationInitData data, Action<float, float> showAverageData)
        {
            if (!CheckBindings()) return;
            EraseSimulationData();

            Debug.Log("<color=green>[MachinePark]</color>" +
                      "<color=blue>[Initialize]</color>" +
                      "<b><color=#003653> Initialization started.</color></b>");

            Debug.Log(
                string.Format("<color=green>[MachinePark]</color><color=blue>[Initialize]</color>" +
                              "<b> Args: Machines number = {0}, Max working time = {1}, " +
                              "Max repairing time = {2}, Approximate points number = {3}</b>",
                              data.MachinesNumberValue, data.MaxWorkingTimeValue,
                              data.MaxRestorationTimeValue, data.ApproximationPointsNumber));

            _machines = new List<GameObject>();
            var range = Mathf.CeilToInt(Mathf.Sqrt(data.MachinesNumberValue));

            var summaryWorkingTime = 0f;
            var summaryRepairingTime = 0f;

            for (var i = 0; i < data.MachinesNumberValue; i++)
            {
                var machine = Instantiate(Resources.Load("Prefabs/Machine")) as GameObject;
                if (machine == null)
                {
                    Debug.LogError("[MachinePark][Initialize] Machine.prefab not found!");
                    break;
                }

                machine.transform.SetParent(transform);
                machine.transform.localScale = Vector3.one;

                var x = (i * 2) % (2 * range) - range;
                var z = range - 2 * (i / range);

                machine.name = string.Format("machine_{0}_{1}", x, z);
                machine.transform.localPosition = new Vector3(x, z, 1);
                machine.transform.localRotation = new Quaternion(-180f, 0f, 0f, 0f);

                var randomPoint = Random.Range(0, data.ApproximationPointsNumber);
                var failureRatio = (float) Math.Round(
                    FailureIntensityCurve.Evaluate(randomPoint == 0 ? randomPoint : 1f / randomPoint), 2);

                var workingTime = (float) Math.Round(failureRatio * data.MaxWorkingTimeValue, 2);
                var repairingTime = Random.Range(0, data.MaxRestorationTimeValue);

                machine.GetComponent<Machine>().Initialize(workingTime, repairingTime);

                summaryWorkingTime += workingTime;
                summaryRepairingTime += repairingTime;

                _machines.Add(machine);
            }

            showAverageData(summaryWorkingTime, summaryRepairingTime);

            Debug.Log("<color=green>[MachinePark]</color>" +
                      "<color=blue>[Initialize]</color>" +
                      "<b><color=#003653> Initialization complete.</color></b>");
        }

        public void StartWork()
        {
            Debug.Log("<color=green>[MachinePark]</color>" +
                      "<color=blue>[StartWork]</color>" +
                      "<b><color=#003653> Simulation started.</color></b>");
            
            if (_machines != null)
                _machines.ForEach(machine =>
                {
                    var component = machine.GetComponent<Machine>();
                    if (component != null) component.StartWork();
                    else
                        Debug.LogError(
                            string.Format("[MachinePark][StartWork] {0} - machine component not found!", machine.name));
                });
        }

        private void EraseSimulationData()
        {
            if (_machines != null && _machines.Any())
                _machines.ForEach(DestroyImmediate);
        }

        private bool CheckBindings()
        {
            if (FailureIntensityCurve == null)
                Debug.LogError("[MachinePark][CheckBindings] Failure intensity curve not assigned!");

            return FailureIntensityCurve != null;
        }
    }
}