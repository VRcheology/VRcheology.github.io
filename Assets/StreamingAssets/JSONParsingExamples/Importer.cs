using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SimpleJSON;
using BestHTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynamoid.SVS
{
	public class Importer : MonoBehaviour 
	{
		JSInterface jsInterface;
		public int version = 1;
		string splitChar = ",";

		string signedURL;
		string assetURL = "http://s3-us-west-2.amazonaws.com/svs-assets/";
		string platformFolder = "webgl/";
		int assetsDownloading = 0;
		bool downloading = false;

		string systemID;
		string systemData;
		Dictionary<string,EnvironmentData> downloadedEnvironments;
		Dictionary<string,ElementData> downloadedElements;
		Dictionary<string,UnityEngine.Object[]> downloadedAssetbundles;
		JObject data;
		string currentEnvironmentID;

		string testJsonPath = "Assets/Data/testSystem.json";

		public void Init (JSInterface js)
		{
			Caching.CleanCache();
			jsInterface = js;

			assetURL = assetURL + js.bundleLocation + "/";

			StartCoroutine(GetSystemData());
		}

		public void TestImport ()
		{
//			assetURL += "webgl/";
			assetURL = "http://www.dynamoidapps.com/SVS/Assets/";
			systemID = "6a6247af-2f36-4758-88ee-198670969986";
			systemData = File.ReadAllText(testJsonPath); 
			ParseSystemData();
			DownloadAssetbundles();
		}

		IEnumerator GetSystemData()
		{
			// TODO put grants service url somewhere more modular
			systemID = jsInterface.uuid;
			string path = "https://market.dynamoid.io/api/systems/" + systemID + "/get_json/";
			var uri = new Uri(path);
			var request = new HTTPRequest(uri);
			request.SetHeader("Authorization", "JWT " + jsInterface.jwt);
			request.Send();

			yield return StartCoroutine(request);

			systemData = request.Response.DataAsText;
			Debug.Log("urlJson = " + systemData);

			ParseSystemData();
			DownloadAssetbundles();
		}

		void ParseSystemData ()
		{
			data = JObject.Parse(systemData);

			currentEnvironmentID = data["currentEnvironmentID"].ToString();

			downloadedAssetbundles = new Dictionary<string, UnityEngine.Object[]>();
			downloadedEnvironments = ParseObjects<EnvironmentData>("environmentIDs", "environments");
			downloadedElements = ParseObjects<ElementData>("elementIDs", "elements");
		}

		Dictionary<string,T> ParseObjects<T> (string idTypeKey, string typeKey) where T : ScriptableObject
		{
			string id;
			Dictionary<string,T> downloadedObjects = new Dictionary<string,T>();
			for (int i = 0; i < (data[idTypeKey] as JArray).Count; i++) 
			{
				id = data[idTypeKey][i].ToString();
				downloadedAssetbundles.Add(id, null);

				T obj = ScriptableObject.CreateInstance<T>();
				JsonUtility.FromJsonOverwrite(data[typeKey][id].ToString(), obj);

				if (!downloadedObjects.ContainsKey(id)) {
					downloadedObjects.Add(id, obj);
				}
			}
			return downloadedObjects;
		}

		void DownloadAssetbundles ()
		{
			foreach (KeyValuePair<string,UnityEngine.Object[]> assetbundle in downloadedAssetbundles) 
			{
				assetsDownloading++;
				StartCoroutine( DownloadAssetbundle(assetbundle.Key) );
			}
			downloading = true;
		}

		IEnumerator DownloadAssetbundle (string assetID)
		{
			while (!Caching.ready) {
				yield return null;
			}

			// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
			using (WWW www = WWW.LoadFromCacheOrDownload (assetURL + assetID, version))
			{
				yield return www;

				if (www.error != null) 
				{
					Debug.Log("Error downloading " + assetID + ": " + www.error);
				}
				else 
				{
					AssetBundle bundle = www.assetBundle;
					downloadedAssetbundles[assetID] = bundle.LoadAllAssets();
					bundle.Unload(false);
				}
				AssetbundleFinishedDownloading();
			}
		}

		void AssetbundleFinishedDownloading ()
		{
			assetsDownloading--;
			if (assetsDownloading < 1 && downloading) 
			{
				foreach (KeyValuePair<string,EnvironmentData> asset in downloadedEnvironments) 
				{
					Debug.Log("Downloaded environment " + asset.Key + "? " + (asset.Value != null));
				}
				foreach (KeyValuePair<string,ElementData> asset in downloadedElements) 
				{
					Debug.Log("Downloaded element " + asset.Key + "? " + (asset.Value != null));
				}
				foreach (KeyValuePair<string,UnityEngine.Object[]> asset in downloadedAssetbundles) 
				{
					Debug.Log("Downloaded assetbundle " + asset.Key + "? " + (asset.Value != null));
				}

				ImportDownloadedData();
				Controller.Instance.GenerateSystem();

				downloading = false;
				assetsDownloading = 0;
			}
		}

		void ImportDownloadedData ()
		{
			float xCoordinate = 0;
			foreach (KeyValuePair<string,EnvironmentData> environmentAsset in downloadedEnvironments)
			{
				Environment environment = new GameObject(environmentAsset.Value.environmentName, typeof(Environment)).GetComponent<Environment>(); 
				environment.environmentData = environmentAsset.Value;
				environment.transform.position = xCoordinate * Vector3.right;
				xCoordinate += Mathf.Max(3f * environment.environmentData.radius, 2000f);

				SetupEnvironment(environment);

				foreach (string elementID in environment.environmentData.elementIDs) 
				{
					SetupElement(elementID, environment);
				}
			}
			Controller.Instance.systemID = systemID;
		}

		void SetupEnvironment (Environment environment)
		{
			if (environment.environmentData.id == currentEnvironmentID) 
			{
				environment.current = true;
			}

			if (AssetBundleHasLoaded(environment.environmentData.id))
			{
				foreach (UnityEngine.Object asset in downloadedAssetbundles[environment.environmentData.id])
				{
					Debug.Log(environment.environmentData.id + ": " + asset.name + " is " + asset.GetType().ToString());
					if (asset.GetType() == typeof(Material)) 
					{
						environment.environmentData.skybox = asset as Material;
					}
					if (asset.GetType() == typeof(Texture)) 
					{
//						Debug.Log(asset.name + " is texture");
					}
				}
			}
		}

		void SetupElement (string elementID, Environment environment)
		{
			ElementData element = downloadedElements[elementID];

			GameObject gameObj = null;
			if (AssetBundleHasLoaded(elementID))
			{
				foreach (UnityEngine.Object asset in downloadedAssetbundles[elementID])
				{
					if (asset.GetType().IsSubclassOf(typeof(DataObject)) || asset.GetType() == typeof(DataObject)) 
					{
						element.dataObject = asset as DataObject;
					}
					else if (asset.GetType() == typeof(GameObject))
					{
						gameObj = asset as GameObject;
					}
				}
			}
			if (element.dataObject != null && element.dataObject.GetType() == typeof(MeshDataObject) && gameObj != null) 
			{
				(element.dataObject as MeshDataObject).model = gameObj;
			}

			if (!string.IsNullOrEmpty(element.targetEnvironmentID)) {
				element.targetEnvironmentData = downloadedEnvironments[element.targetEnvironmentID];
			}

			EnvironmentElement environmentElement = environment.environmentData.elements.Find( e => e.elementID == elementID );
			if (environmentElement != null) {
				environmentElement.elementData = element;
			}
		}

		bool AssetBundleHasLoaded (string assetID)
		{
			return (!string.IsNullOrEmpty(assetID) && downloadedAssetbundles.ContainsKey(assetID) && downloadedAssetbundles[assetID] != null);
		}
	}
}
