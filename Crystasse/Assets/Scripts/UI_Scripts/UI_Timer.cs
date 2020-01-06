using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditorInternal;
using UnityEngine;


namespace CustomUI
{
    public class UI_Timer : MonoBehaviourPunCallbacks
    {
        #region Variables / Properties
        private float time, m, s;
        private string hours, minutes, seconds;
        public bool timer = false;

        #endregion

        #region Methods

        void Update()
        {
            //if (Input.GetKey(KeyCode.F))
            //{
            //    StartTimer();
            //}
            Date();
            if (timer)
            {
                RunTimer(this.time);
                WriteTime(TimeFormatter(this.time));
            }

        }

        public void StartTimer()
        {
            timer = true;
            this.time = 0f;
            RunTimer(this.time);
        }

        void RunTimer(float actualTimer)
        {
            //timer = true;
            Time.timeScale = 1f;
            this.time += Time.deltaTime;
        }
        void StopTimer()
        {
            Time.timeScale = 0f;
            timer = false;
        }


        public string TimeFormatter()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(this.time);
            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            return formattedTime;
        }
        public string TimeFormatter(float actualTimer)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(actualTimer);
            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            return formattedTime;
        }

        void WriteTime(string formattedTime)
        {
            GameManager.MasterManager.UIManager._TimeRunning.text = formattedTime;
        }

        void Date()
        {
            GameManager.MasterManager.UIManager._RealTime.text = System.DateTime.Now.ToString();
        }

        #endregion
    }
}
