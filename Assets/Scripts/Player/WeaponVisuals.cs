using UnityEngine;

namespace BackroomsShooter.Player
{
    public class WeaponVisuals : MonoBehaviour
    {
        public Transform FirePoint;
        private GameObject _currentModel;

        private void Start()
        {
            PlayerResources.OnResourcesChanged += RefreshVisuals;
            RefreshVisuals();
        }

        private void OnDestroy()
        {
            PlayerResources.OnResourcesChanged -= RefreshVisuals;
        }

        public void RefreshVisuals()
        {
            var res = GetComponent<PlayerResources>();
            if (res == null || res.CurrentWeaponData == null) return;

            if (_currentModel != null) Destroy(_currentModel);

            if (res.CurrentWeaponData.WeaponModelPrefab != null)
            {
                _currentModel = Instantiate(res.CurrentWeaponData.WeaponModelPrefab, FirePoint);
                _currentModel = Instantiate(res.CurrentWeaponData.WeaponModelPrefab, FirePoint);
                _currentModel.transform.localPosition = Vector3.zero;
                _currentModel.transform.localRotation = Quaternion.identity;
            }
        }
    }
}