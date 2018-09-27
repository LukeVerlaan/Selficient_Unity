using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using Task;
using Business.Domain;

namespace Business {
	
	public class AreaService : MonoBehaviour{
        //string uri = "https://server.ecliptic.nl/hu/nosi/service/getallhardware?apikey=kdfjadslj2xk";
        private UnityAction<System.Object> saveHardwareState;
		private UnityAction<System.Object> loadHardwareDataset;

		[SerializeField] private string uri = "https://server.ecliptic.nl/hu/nosi/service/getallhardware?apikey=kdfjadslj2xk";
		[SerializeField] string stateUri = "https://server.ecliptic.nl/hu/nosi/serivice/updatestate?apikey=kdfjadslj2xk";
		[SerializeField] string datasetUri = "https://server.ecliptic.nl/hu/nosi/newdashboard/api/graph";
		[SerializeField] private string areaId; // TODO: De NoSi zo aanpassen dat er per area in geladen kan worden.
		private IDataService serviceImplementation;
		// Use this for initialization

		void Awake(){
			saveHardwareState = new UnityAction<System.Object> (SaveHardwareState);
			loadHardwareDataset = new UnityAction<System.Object> (LoadHardwareDataset);
            Debug.LogError("Awake");
        }
		void OnEnable(){    
			EventManager.StartListening ("loadHardwareDataset", LoadHardwareDataset);
			EventManager.StartListening ("updateHardwareState", SaveHardwareState);
            Debug.LogError("onEnable");

        }
		void OnDisable(){
			EventManager.StopListening ("loadHardwareDataset", LoadHardwareDataset);
			EventManager.StopListening ("updateHardwareState", SaveHardwareState);
            Debug.LogError("onDisable");

        }
		void Start () {
			serviceImplementation = new HardwareService ();
			StartCoroutine(AreaLoader(uri));
            Debug.LogError("Start");
		}
	
		#region Hier worden alle data requests afgehandeld.
		private IEnumerator AreaLoader (string uri) // TODO : Hier dus een areaId aan mee geven.
		{
			return this.serviceImplementation.AreaLoader (uri);
		}

		private void SaveHardwareState (System.Object hardwareObject)
		{
			EventManager.TriggerEvent ("showInteractiveLoader", true);
			StartCoroutine (this.serviceImplementation.SaveHardwareState (hardwareObject, stateUri));
		}
		#endregion

		private void LoadHardwareDataset (System.Object dataSetId)
		{
            string stringDatasetId = dataSetId as string;
			EventManager.TriggerEvent ("showInteractiveLoader", true);
			StartCoroutine (this.serviceImplementation.LoadHardwareDataset (stringDatasetId, datasetUri));
            Debug.LogError("LoadHardwareDataset...");
		}

	
	}

}
