using UnityEngine;

namespace MPSPrototype
{
    public class Machine : MonoBehaviour
    {
        private float _workingTime;
        private float _repairingTime;
        private float _currentTime;
        private bool _isWorking;

        public void Initialize(float workingTime, float repairingTime)
        {
            _workingTime = workingTime;
            _repairingTime = repairingTime;
        }

        private void Update()
        {
            _currentTime -= Time.deltaTime;
            if (!(_currentTime <= 0)) return;
            if (_isWorking)
                StartResoration();
            else
                StartWork();
        }

        public void StartWork()
        {
            _currentTime = _workingTime;
            GetComponent<MeshRenderer>().material.color = Color.green;
            _isWorking = true;
        }

        public void StartResoration()
        {
            _currentTime = _repairingTime;
            GetComponent<ParticleSystem>().Play();
            GetComponent<MeshRenderer>().material.color = Color.red;
            _isWorking = false;
        }
    }
}