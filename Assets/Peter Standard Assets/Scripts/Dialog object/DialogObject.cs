using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EnliStandardAssets.Localization;

namespace EnliStandardAssets.Dialogs
{
    public class DialogObject : MonoBehaviour
    {
        [SerializeField] private DialogObjectGUI dialogGUI;

        [SerializeField] private Text titleText;
        [SerializeField] private Text messageText;
        [SerializeField] private Toggle dialogToggle;
        [SerializeField] private Text dialogToggleText;
        [SerializeField] private GameObject neutralButton;
        [SerializeField] private GameObject negativeButton;
        [SerializeField] private GameObject positiveButton;
        [SerializeField] private GameObject cancelButton;
        [SerializeField] private Text neutralButtonText;
        [SerializeField] private Text negativeButtonText;
        [SerializeField] private Text positiveButtonText;
        [SerializeField] private Text cancelButtonText;
        public bool canClickBackgroundToCancel = true;

        [Header("Translations")]
        [SerializeField] private GameObject localizationProviderObject;

        private GameObject threeButtonsParent;
        private Builder activeBuilder;
        private Queue<Builder> dialogTaskQueue = new Queue<Builder>();
        private bool dialogIsShowing = false;
        private ILocalizationProvider localizationProvider;

        private string[] multipleButtons;

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern int _ShowAlertMessage(string callbackObject, string title, string message, string neutralText, string negativeText, string positiveButtonText, string cancelButtonText);
#endif

        public string YES { get; private set; } = "Áno";
        public string NO { get; private set; } = "Nie";
        public string OK { get; private set; } = "OK";
        public string CANCEL { get; private set; } = "Zrušiť";
        public string SETTINGS { get; private set; } = "Nastavenia";
        public string GENERAL_ERROR_TITLE { get; private set; } = "Chyba";
        public string GENERAL_ERROR { get; private set; } = "Vyskytla sa chyba";

        #region Translating

        private void TranslateItems()
        {
            if (localizationProvider == null)
                return;
            
            YES = localizationProvider.Translate("Dialog object/yes", YES);
            NO = localizationProvider.Translate("Dialog object/no", NO);
            OK = localizationProvider.Translate("Dialog object/ok", OK);
            CANCEL = localizationProvider.Translate("Dialog object/cancel", CANCEL);
            SETTINGS = localizationProvider.Translate("Dialog object/settings", SETTINGS);
            GENERAL_ERROR_TITLE = localizationProvider.Translate("Dialog object/general_error_title", GENERAL_ERROR_TITLE);
            GENERAL_ERROR = localizationProvider.Translate("Dialog object/general_error", GENERAL_ERROR);
        }
#endregion

        void Awake()
        {
            threeButtonsParent = positiveButton.transform.parent.gameObject;

            if (localizationProviderObject != null)
                localizationProviderObject?.GetInterface(out localizationProvider);
            if (localizationProvider != null)
                localizationProvider.LanguageChanged += TranslateItems;
            TranslateItems();
        }

        private void OnValidate()
        {
            if (localizationProviderObject != null)
            {
                localizationProviderObject.GetInterface(out localizationProvider);
                if (localizationProvider == null)
                    localizationProviderObject = null;
            }
        }

        private void Open()
        {
            dialogGUI.Open();
        }

        private void Close()
        {
            dialogGUI.Close();
            dialogIsShowing = false;
            ShowNextDialogInQueue();
        }

        private void SetDialogWindow(Builder dialogBuilder)
        {
            titleText.text = dialogBuilder.Title;
            messageText.text = dialogBuilder.Message;
            dialogToggleText.text = dialogBuilder.ToggleText;

            if (dialogBuilder.HasCancelButton)
            {
                if (!dialogBuilder.HasNegativeButton)
                {
                    dialogBuilder.SetNegativeButton(dialogBuilder.CancelButtonText, dialogBuilder.CancelCallback);
                    dialogBuilder.SetCancelButton(string.Empty, null);
                }
                else if (!dialogBuilder.HasNeutralButton)
                {
                    dialogBuilder.SetNeutralButton(dialogBuilder.CancelButtonText, dialogBuilder.CancelCallback);
                    dialogBuilder.SetCancelButton(string.Empty, null);
                }
            }

            positiveButtonText.text = dialogBuilder.PositiveButtonText;
            positiveButton.SetActive(dialogBuilder.HasPositiveButton);

            negativeButtonText.text = dialogBuilder.NegativeButtonText;
            negativeButton.SetActive(dialogBuilder.HasNegativeButton);

            neutralButtonText.text = dialogBuilder.NeutralButtonText;
            neutralButton.SetActive(dialogBuilder.HasNeutralButton);

            cancelButtonText.text = dialogBuilder.CancelButtonText;
            cancelButton.SetActive(dialogBuilder.HasCancelButton);

            dialogToggleText.text = dialogBuilder.ToggleText;
            dialogToggle.gameObject.SetActive(dialogBuilder.HasToggle);

            threeButtonsParent.SetActive(dialogBuilder.HasPositiveButton || dialogBuilder.HasNegativeButton || dialogBuilder.HasNeutralButton);
        }

        public void OnPositiveButtonClicked()
        {
            Close();
            if (activeBuilder.PositiveCallback != null)
                activeBuilder.PositiveCallback(dialogToggle.isOn);
        }

        public void OnNeutralButtonClicked()
        {
            Close();
            if (activeBuilder.NeutralCallback != null)
                activeBuilder.NeutralCallback(dialogToggle.isOn);
        }

        public void OnNegativeButtonClicked()
        {
            Close();
            if (activeBuilder.NegativeCallback != null)
                activeBuilder.NegativeCallback(dialogToggle.isOn);
        }

        public void OnCancelButtonClicked()
        {
            Close();
            if (activeBuilder.CancelCallback != null)
                activeBuilder.CancelCallback(dialogToggle.isOn);
        }

        public void OnBackgroundClicked()
        {
            if (canClickBackgroundToCancel)
                OnCancelButtonClicked();
        }

        public void AddToQueue(Builder dialogWindow)
        {
            dialogTaskQueue.Enqueue(dialogWindow);
            ShowNextDialogInQueue();
        }

        private void ShowNextDialogInQueue()
        {
            if (dialogTaskQueue.Count > 0 && !dialogIsShowing)
            {
                activeBuilder = dialogTaskQueue.Dequeue();
                dialogIsShowing = true;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
                SetDialogWindow(activeBuilder);
                Open();
#elif UNITY_ANDROID
            ShowAndroidDialog(activeBuilder);
#elif UNITY_IOS
			ShowIOSDialog(activeBuilder);
#endif
            }
        }
        public class Builder
        {
            public string Title { get; private set; }
            public string Message { get; private set; }
            public string ToggleText { get; private set; }
            public string PositiveButtonText { get; private set; }
            public string NeutralButtonText { get; private set; }
            public string NegativeButtonText { get; private set; }
            public string CancelButtonText { get; private set; }

            public bool HasPositiveButton { get { return !string.IsNullOrEmpty(PositiveButtonText); } }
            public bool HasNegativeButton { get { return !string.IsNullOrEmpty(NegativeButtonText); } }
            public bool HasNeutralButton { get { return !string.IsNullOrEmpty(NeutralButtonText); } }
            public bool HasCancelButton { get { return !string.IsNullOrEmpty(CancelButtonText); } }
            public bool HasToggle { get { return !string.IsNullOrEmpty(ToggleText); } }

            public Action<bool> PositiveCallback { get; private set; }
            public Action<bool> NeutralCallback { get; private set; }
            public Action<bool> NegativeCallback { get; private set; }
            public Action<bool> CancelCallback { get; private set; }

            private DialogObject context;

            public Builder(DialogObject context)
            {
                this.context = context;
                Title = Message = ToggleText = string.Empty;
                PositiveButtonText = NeutralButtonText = NegativeButtonText = CancelButtonText = string.Empty;
                PositiveCallback = NeutralCallback = NegativeCallback = CancelCallback = null;
            }

            public Builder SetTitle(string title)
            {
                Title = title;
                return this;
            }

            public Builder SetMessage(string message)
            {
                Message = message;
                return this;
            }

            public Builder SetToggleText(string text)
            {
                ToggleText = text;
                return this;
            }

            public Builder SetPositiveButton(string text, Action<bool> callback)
            {
                PositiveButtonText = text;
                PositiveCallback = callback;
                return this;
            }

            public Builder SetNeutralButton(string text, Action<bool> callback)
            {
                NeutralButtonText = text;
                NeutralCallback = callback;
                return this;
            }

            public Builder SetNegativeButton(string text, Action<bool> callback)
            {
                NegativeButtonText = text;
                NegativeCallback = callback;
                return this;
            }

            public Builder SetCancelButton(string text, Action<bool> callback)
            {
                CancelButtonText = text;
                CancelCallback = callback;
                return this;
            }

            public void Show()
            {
                context.AddToQueue(this);
            }
        }

#if UNITY_ANDROID
        public void ShowAndroidDialog(Builder dialogBuilder)
        {
            using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var androidPlugin = new AndroidJavaObject("com.enli.unity_plugins.DialogPlugin", currentActivity, gameObject.name))
            {
                if (multipleButtons == null)
                    androidPlugin.Call("ShowAlertDialog", dialogBuilder.Title, dialogBuilder.Message,
                            dialogBuilder.NeutralButtonText, dialogBuilder.NegativeButtonText, dialogBuilder.PositiveButtonText, dialogBuilder.CancelButtonText);
                else
                    androidPlugin.Call("ShowAlertDialog", dialogBuilder.Title, dialogBuilder.Message, multipleButtons);
            }
        }
#endif

#if UNITY_IOS
        private void ShowIOSDialog(Builder dialogBuilder)
        {
            //Debug.Log(string.Format("GO name: {0}, title: {1}, message: {2}, neutral: {3}", gameObject.name, dialogBuilder.Title, dialogBuilder.Message, dialogBuilder.NeutralButtonText));
            _ShowAlertMessage(gameObject.name,
                dialogBuilder.Title, dialogBuilder.Message, 
                dialogBuilder.NeutralButtonText, dialogBuilder.NegativeButtonText, dialogBuilder.PositiveButtonText, dialogBuilder.CancelButtonText);
        }
#endif

        public void CallbackFromOutside(string message)
        {
            dialogIsShowing = false;
            int buttonIndex = int.Parse(message);
            //Debug.Log ("CallbackFromOutside " + buttonIndex);
            switch (buttonIndex)
            {
                case 0:
                    if (activeBuilder.CancelCallback != null)
                        activeBuilder.CancelCallback(false);
                    break;
                case 1:
                    if (activeBuilder.NeutralCallback != null)
                        activeBuilder.NeutralCallback(false);
                    break;
                case 2:
                    if (activeBuilder.NegativeCallback != null)
                        activeBuilder.NegativeCallback(false);
                    break;
                case 3:
                    if (activeBuilder.PositiveCallback != null)
                        activeBuilder.PositiveCallback(false);
                    break;
            }

            ShowNextDialogInQueue();
        }
    }
}