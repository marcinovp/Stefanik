package com.enli.unity_plugins;

import android.app.Activity;
import android.content.Context;
import android.os.Handler;
import android.widget.Toast;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

@SuppressWarnings("unused")
public class ToastPlugin {

    private Context context;

    public ToastPlugin(Context context)
    {
        this.context = context;
    }

    public void ShowToast(final String toastMessage, final boolean longToast)
    {
        Handler mainHandler = new Handler(context.getMainLooper());

        Runnable myRunnable = new Runnable() {
            @Override
            public void run() {
                int toastLength = longToast ? Toast.LENGTH_LONG : Toast.LENGTH_SHORT;
                Toast.makeText(context, toastMessage, toastLength).show();
            }
        };
        mainHandler.post(myRunnable);
    }
}