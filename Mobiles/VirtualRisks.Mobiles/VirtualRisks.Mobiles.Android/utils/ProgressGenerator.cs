﻿using System;
using Android.App;
using Android.OS;
using Library;

namespace VirtualRisks.Mobiles.Droid.utils
{

	public class ProgressGenerator
	{
		Action action;
		Handler messageHandler = new Handler();
		public interface OnCompleteListener {

			void onComplete();
		}

		private OnCompleteListener mListener;
		private int mProgress;

		public ProgressGenerator(OnCompleteListener listener) {
			mListener = listener;
		}
		public void start(ProcessButton button,Activity activity) {
 
			activity.RunOnUiThread (() => {
				action = ()=> UpdateProgress(button,0);
				messageHandler.PostDelayed(action,generateDelay());
			}); 
		}
		private Random random = new Random();

		void UpdateProgress(ProcessButton button,int progress){

			mProgress += 10;
			button.setProgress(mProgress);
			if (mProgress < 100) {
				Console.WriteLine("Progress "+mProgress);
				action = ()=> UpdateProgress(button,mProgress);
				messageHandler.PostDelayed(action, generateDelay());
			} else {
				mListener.onComplete();
				Console.WriteLine("Progress Completed "+mProgress);

			}
		}

		private int generateDelay() {
			return random.Next(1000);
		}
	}
}

