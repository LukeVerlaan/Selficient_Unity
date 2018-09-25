﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Presentation.Dashboard;
using Business.Domain;
using Task;
using UnityEngine.Rendering.PostProcessing;
using Assets.Software.Presentation_layer.Camera.Effect;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;

namespace Presentation {

    public class DashboardInteraction : Interactable {

        GameObject sensor;
        Hardware hardware;
        private UnityAction<System.Object> hideDashboard;

        private DashboardRenderer renderer;
        private GameObject dashboard;
        private Grayscale grayscaleEffect;
        public DashboardInteraction(GameObject sensor, Hardware hardware) {
            this.sensor = sensor;
            this.hardware = hardware;

            this.renderer = new DashboardRenderer();
        }

        #region Interactable implementation

        public void On()
        {
            grayscaleEffect.enabled.value = true;
            Debug.Log("Show dashboard " + this.sensor.name);
            renderer.RenderContent(); // renderd niet opnieuw als het al eens geladen is.
            dashboard.SetActive(true);
        }

        public void Off()
        {
            grayscaleEffect.enabled.value = false;
            Debug.Log("Hide dashboard " + this.sensor.name);
            dashboard.SetActive(false);
        }

        public void Init()
        {
            EventManager.StartListening("hideDashboard", PutOffDashboard);
            PostProcessVolume volume = GameObject.Find("Post-process Volume").GetComponent<PostProcessVolume>();
            volume.profile.TryGetSettings(out grayscaleEffect);
            //throw new System.NotImplementedException ();
            renderer = new DashboardRenderer();
            renderer.InitializeDashboard(this.sensor, this.hardware);
            dashboard = renderer.GetDashboard();
            dashboard.SetActive(false);

        }

        private void PutOffDashboard(System.Object arg)
        {
            this.Off();
        }

        public bool WantsUpdate()
        {
            return false;
        }
        #endregion



    }
}