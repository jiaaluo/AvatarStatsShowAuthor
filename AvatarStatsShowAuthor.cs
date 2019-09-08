using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRCModLoader;
using VRCTools;

namespace AvatarStatsShowAuthor
{
    public static class ModInfo
    {
        public const string Name = "AvatarStatsShowAuthor";
        public const string Author = "Herp Derpinstine";
        public const string Company = "NanoNuke @ nanonuke.net";
        public const string Version = "1.0.0";
    }
    [VRCModInfo(ModInfo.Name, ModInfo.Version, ModInfo.Author)]

    public class AvatarStatsShowAuthor : VRCMod
    {
        private static VRC.UI.PageAvatar pageAvatar;
        private static VRC.UI.PageUserInfo pageUserInfo;

        void OnApplicationStart() { ModManager.StartCoroutine(WaitForUIManager()); }

        IEnumerator WaitForUIManager()
        {
            yield return VRCUiManagerUtils.WaitForUiManagerInit();
            GameObject pageUserInfoObj = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo");
            if (pageUserInfoObj != null)
            {
                pageUserInfo = pageUserInfoObj.GetComponent<VRC.UI.PageUserInfo>();
                if (pageUserInfo != null)
                {
                    pageAvatar = Resources.FindObjectsOfTypeAll<VRC.UI.PageAvatar>().First(p => (p.transform.Find("Change Button") != null));
                    if (pageAvatar != null)
                    {
                        GameObject statspopup = GameObject.Find("UserInterface/MenuContent/Popups/AvatarStatsPopup");
                        if (statspopup != null)
                        {
                            Transform documentationbutton = statspopup.transform.Find("AvatarStatsMenu/_Buttons/DocumentationButton");
                            if (documentationbutton != null)
                            {
                                RectTransform recttrans = documentationbutton.GetComponent<RectTransform>();
                                if (recttrans != null)
                                {
                                    recttrans.sizeDelta = new Vector2((recttrans.sizeDelta.x - 600), recttrans.sizeDelta.y);
                                    recttrans.localPosition = new Vector3((recttrans.localPosition.x - 300), recttrans.localPosition.y, recttrans.localPosition.z);
                                }

                                Transform buttontxt_trans = documentationbutton.Find("Text");
                                if (buttontxt_trans != null)
                                {
                                    Text buttontxt = buttontxt_trans.GetComponent<Text>();
                                    if (buttontxt != null)
                                        buttontxt.fontSize -= 10;
                                }

                                Transform buttonoverheadtxt_trans = documentationbutton.Find("Text (1)");
                                if (buttonoverheadtxt_trans != null)
                                {
                                    RectTransform recttranstxt = buttonoverheadtxt_trans.GetComponent<RectTransform>();
                                    if (recttranstxt != null)
                                    {
                                        recttranstxt.sizeDelta = new Vector2((recttranstxt.sizeDelta.x + 600), recttranstxt.sizeDelta.y);
                                        recttranstxt.localPosition = new Vector3((recttranstxt.localPosition.x + 300), recttranstxt.localPosition.y, recttranstxt.localPosition.z);
                                    }
                                }

                                Transform showcreatorbutton_trans = UnityUiUtils.DuplicateButton(documentationbutton, "Show Avatar Author", new Vector2(600, 0));
                                Button showcreatorbutton = showcreatorbutton_trans.GetComponent<Button>();
                                showcreatorbutton.onClick = new Button.ButtonClickedEvent();
                                showcreatorbutton.onClick.AddListener(() =>
                                {
                                    if ((pageUserInfo != null) && (pageAvatar != null) && (pageAvatar.avatar != null))
                                    {
                                        VRC.Core.ApiAvatar currentApiAvatar = pageAvatar.avatar.apiAvatar;
                                        if (currentApiAvatar != null)
                                        {
                                            string authorid = currentApiAvatar.authorId;
                                            if (!string.IsNullOrEmpty(authorid))
                                            {
                                                VRC.Core.APIUser.FetchUser(authorid, (VRC.Core.APIUser user) =>
                                                {
                                                    VRCUiManagerUtils.GetVRCUiManager().ShowScreen(pageUserInfo);
                                                    pageUserInfo.SetupUserInfo(user);
                                                    VRCUiPopupManagerUtils.GetVRCUiPopupManager().HideCurrentPopup();
                                                }, null);
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}
