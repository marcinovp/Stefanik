package com.enli.unity_plugins;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.os.Handler;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

@SuppressWarnings("unused")
public class DialogPlugin {

    private Context context;

    final String CALLBACK_OBJECT;
    final String CALLBACK_METHOD = "CallbackFromOutside";

    public DialogPlugin(Context context, String callbackObject)
    {
        this.context = context;
        CALLBACK_OBJECT = callbackObject;
    }

    public void ShowAlertDialog(final String title, final String message, final String neutralButton, final String negativeButton, final String positiveButton, final String cancelButton )
    {
        Handler mainHandler = new Handler(context.getMainLooper());
        Runnable myRunnable = new Runnable() {
            @Override
            public void run() {
                //Kvoli tomu aby bola logika konzistentna s ios
                final String[] callbackCodes = new String[]{"0", "1", "2", "3"};
                String neutralButton2 = neutralButton;
                String negativeButton2 = negativeButton;

                if (cancelButton != null && cancelButton.length() > 0) {
                    if (negativeButton == null || negativeButton.length() == 0) {
                        negativeButton2 = cancelButton;
                        callbackCodes[2] = "0";
                    }
                    else if (neutralButton == null || neutralButton.length() == 0) {
                        neutralButton2 = cancelButton;
                        callbackCodes[1] = "0";
                    }
                }

                AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(context)
                        .setTitle(title)
                        .setMessage(message);

                //neutralButton button
                if (neutralButton2 != null && neutralButton2.length() > 0)
                    dialogBuilder.setNeutralButton(neutralButton2, new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            AndroidToUnityCall(CALLBACK_OBJECT, CALLBACK_METHOD, callbackCodes[1]);
                        }
                    });

                //negativeButton button
                if (negativeButton2 != null && negativeButton2.length() > 0)
                    dialogBuilder.setNegativeButton(negativeButton2, new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            AndroidToUnityCall(CALLBACK_OBJECT, CALLBACK_METHOD, callbackCodes[2]);
                        }
                    });

                //positive button
                if (positiveButton != null && positiveButton.length() > 0)
                    dialogBuilder.setPositiveButton(positiveButton, new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            AndroidToUnityCall(CALLBACK_OBJECT, CALLBACK_METHOD, callbackCodes[3]);
                        }
                    });
                dialogBuilder.setOnCancelListener(new DialogInterface.OnCancelListener() {
                    @Override
                    public void onCancel(DialogInterface dialog) {
                        AndroidToUnityCall(CALLBACK_OBJECT, CALLBACK_METHOD, callbackCodes[0]);
                    }
                });

                dialogBuilder.show();
            }
        };
        mainHandler.post(myRunnable);
    }

    public void ShowAlertDialog(final String title, final String[] choices)
    {
        Handler mainHandler = new Handler(context.getMainLooper());
        Runnable myRunnable = new Runnable() {
            @Override
            public void run() {
                AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(context)
                        .setTitle(title);

                dialogBuilder.setItems(choices, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        AndroidToUnityCall(CALLBACK_OBJECT, CALLBACK_METHOD, String.valueOf(which+1));
                    }
                });

                dialogBuilder.create().show();
            }
        };
        mainHandler.post(myRunnable);
    }

    private void AndroidToUnityCall(String unityObject, String method, String message)
    {
        UnityPlayer.UnitySendMessage(unityObject, method, message);
    }
}