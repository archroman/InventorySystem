using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    internal sealed class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;

        [SerializeField] private GameObject _lockOverlay;
        [SerializeField] private GameObject _unlockButton;
        [SerializeField] private TMP_Text _costText;

        private Action _onUnlock;

        public void BindUnlock(Action callback)
        {
            _onUnlock = callback;
        }

        public void OnUnlockClicked()
        {
            _onUnlock?.Invoke();
        }

        public void ShowLocked(int cost)
        {
            _lockOverlay.SetActive(true);

            _unlockButton.SetActive(true);
            _costText.gameObject.SetActive(true); 
            _costText.text = cost.ToString();

            _icon.enabled = false;
            _count.text = "";
        }

        public void ShowItem(Sprite icon, int count)
        {
            _lockOverlay.SetActive(false);

            _unlockButton.SetActive(false);
            _costText.gameObject.SetActive(false);  

            _icon.enabled = true;
            _icon.sprite = icon;

            _count.text = count > 1 ? count.ToString() : "";
        }

        public void ShowEmpty()
        {
            _lockOverlay.SetActive(false);

            _unlockButton.SetActive(false);
            _costText.gameObject.SetActive(false);  

            _icon.enabled = false;
            _count.text = "";
        }
    }
}