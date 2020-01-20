using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;
using PUN_Network;
using System.Linq;

namespace Michsky.UI.ModernUIPack
{
    public class CustomDropdown : MonoBehaviour, IPunObservable
    {
        [Header("OBJECTS")]
        public GameObject triggerObject;
        public TextMeshProUGUI selectedText;
        //TODO: set image of Unit
        public Image selectedImage;
        public Transform itemParent;
        public GameObject itemObject;
        public GameObject scrollbar;
        private VerticalLayoutGroup itemList;

        [Header("SETTINGS")]
        public bool enableIcon = true;
        public bool enableTrigger = true;
        public bool enableScrollbar = true;
        public bool invokeAtStart = false;
        public AnimationType animationType;

        [Space(20)]
        [SerializeField]
        public List<Item> dropdownItems = new List<Item>();
        private List<Item> imageList = new List<Item>();
        //TODO: selectedIndex pick out
        public int selectedItemIndex = 0;
        [Space(20)]

        private Animator dropdownAnimator;
        private TextMeshProUGUI setItemText;
        private Image setItemImage;

        Sprite imageHelper;
        string textHelper;
        bool isOn;

        public enum AnimationType
        {
            FADING,
            SLIDING,
            STYLISH
        }

        [System.Serializable]
        public class Item
        {
            public string itemName = "Dropdown Item";
            public Sprite itemIcon;
            public UnityEvent OnItemSelection;
        }

        void Start()
        {
            dropdownAnimator = this.GetComponent<Animator>();
            itemList = itemParent.GetComponent<VerticalLayoutGroup>();

            for (int i = 0; i < dropdownItems.Count; ++i)
            {
                GameObject go = Instantiate(itemObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(itemParent, false);

                setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                textHelper = dropdownItems[i].itemName;
                setItemText.text = textHelper;

                Transform goImage;
                goImage = go.gameObject.transform.Find("Icon");
                setItemImage = goImage.GetComponent<Image>();
                imageHelper = dropdownItems[i].itemIcon;
                setItemImage.sprite = imageHelper;

                Button itemButton;
                itemButton = go.GetComponent<Button>();
                itemButton.onClick.AddListener(dropdownItems[i].OnItemSelection.Invoke);
                itemButton.onClick.AddListener(Animate);

                if (invokeAtStart == true)
                {
                    dropdownItems[i].OnItemSelection.Invoke();
                }
            }

            selectedText.text = dropdownItems[selectedItemIndex].itemName;
            selectedImage.sprite = dropdownItems[selectedItemIndex].itemIcon;

            if (enableScrollbar == true)
            {
                itemList.padding.right = 25;
                scrollbar.SetActive(true);
            }

            else
            {
                itemList.padding.right = 8;
                Destroy(scrollbar);
            }

            if (enableIcon == false)
            {
                selectedImage.enabled = false;
            }
        }

        public void ChangeDropdownInfo(int itemIndex)
        {
            GameManager.MasterManager.teamToPlayer[(byte)(selectedItemIndex + 1)] = null;
            GameManager.MasterManager.teamToPlayer[(byte)(itemIndex + 1)] = GetReliantCustomPlayer().photonView.Owner;
            Debug.Log($"GetReliantCustomPlayer().photonView.Owner: {GetReliantCustomPlayer().photonView.Owner}");
            GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID = (byte)(itemIndex + 1);
            GameManager.MasterManager.InputManager._teamID = (byte)(itemIndex + 1);
            GameManager.MasterManager.NetworkManager.CustomPlayer.PlayerlistEntry.PlayerTeam = (itemIndex + 1);
            selectedImage.sprite = dropdownItems[itemIndex].itemIcon;
            selectedText.text = dropdownItems[itemIndex].itemName;
            selectedItemIndex = itemIndex;
            Selection.TeamID = (byte)(itemIndex + 1);

            // dropdownItems[itemIndex].OnItemSelection.Invoke();
        }

        public void ChangeDropdownInfoChecked(int itemIndex)
        {

            if (GetReliantCustomPlayer().IsMyCustomPlayer)
            {
                ChangeDropdownInfo(itemIndex);
                GetReliantCustomPlayer().PlayerlistEntry.photonView.RPC("RPC_ChangeIcon", RpcTarget.AllBufferedViaServer, itemIndex);
                //if (Constants.UNIT_ICONS.Length > itemIndex)
                //{
                //    selectedImage.sprite = Resources.Load<Sprite>(Constants.UNIT_ICONS[itemIndex]);
                //}
                //else
                //{
                //    Debug.Log("UnitIcon does not exist!");
                //    selectedImage.sprite = Resources.Load<Sprite>(Constants.UNIT_ICONS[0]);
                //}
                //GameManager.MasterManager.NetworkManager.CustomPlayer.TeamID = (byte)(itemIndex + 1);
                //GameManager.MasterManager.InputManager._teamID = (byte)(itemIndex + 1);
                //GameManager.MasterManager.NetworkManager.CustomPlayer.PlayerlistEntry.PlayerTeam = (itemIndex + 1);
                //selectedImage.sprite = dropdownItems[itemIndex].itemIcon;
                //selectedText.text = dropdownItems[itemIndex].itemName;
                //selectedItemIndex = itemIndex;
                //Selection.TeamID = (byte)(itemIndex + 1);
            }
            // dropdownItems[itemIndex].OnItemSelection.Invoke();
        }

        public PUN_CustomPlayer GetReliantCustomPlayer()
        {
            foreach (KeyValuePair<PUN_CustomPlayer, GameObject> kvp in GameManager.MasterManager.NetworkManager._playerListEntries)
            {
                if (kvp.Value == this.GetComponentInParent<PUN_PlayerlistEntry>().gameObject)
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        public void Animate()
        {
            if (isOn == false && animationType == AnimationType.FADING)
            {
                dropdownAnimator.Play("Fading In");
                isOn = true;
            }

            else if (isOn == true && animationType == AnimationType.FADING)
            {
                dropdownAnimator.Play("Fading Out");
                isOn = false;
            }

            else if (isOn == false && animationType == AnimationType.SLIDING)
            {
                dropdownAnimator.Play("Sliding In");
                isOn = true;
            }

            else if (isOn == true && animationType == AnimationType.SLIDING)
            {
                dropdownAnimator.Play("Sliding Out");
                isOn = false;
            }

            else if (isOn == false && animationType == AnimationType.STYLISH)
            {
                dropdownAnimator.Play("Stylish In");
                isOn = true;
            }

            else if (isOn == true && animationType == AnimationType.STYLISH)
            {
                dropdownAnimator.Play("Stylish Out");
                isOn = false;
            }

            if (enableTrigger == true && isOn == false)
            {
                triggerObject.SetActive(false);
            }

            else if (enableTrigger == true && isOn == true)
            {
                triggerObject.SetActive(true);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                //stream.SendNext(selectedImage.sprite);
                stream.SendNext(selectedText.text);
                stream.SendNext(selectedItemIndex);

                //Debug.Log($"LocalClient sending dropDownInfo {GetComponent<PhotonView>().ViewID}");
            }
            else
            {
                //this.selectedImage.sprite = (Sprite)stream.ReceiveNext();
                this.selectedText.text = (string)stream.ReceiveNext();
                this.selectedItemIndex = (int)stream.ReceiveNext();
                //Debug.Log($"LocalClient receiving dropDownInfo {GetComponent<PhotonView>().ViewID}");
            }
        }
    }
}