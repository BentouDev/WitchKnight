using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorPrefab : MonoBehaviour
{
	public GameObject Prefab;
	private GameObject Instance;

	void OnValidate()
	{
		if (!Instance)
			Instance = Instantiate(Prefab, transform);
	}

	void Start()
	{
		if (!Instance)
			Instance = Instantiate(Prefab, transform);
	}
	
	EditorPrefab()
	{
		PrefabUtility.prefabInstanceUpdated -= PrefabInstanceUpdated;
		PrefabUtility.prefabInstanceUpdated += PrefabInstanceUpdated;
	}
 
	private static void PrefabInstanceUpdated(GameObject root)
	{
		if (!Application.isPlaying)
		{
			bool needReApply = false;
 
			var prefab = root.GetComponent<EditorPrefab>();
			if (prefab && prefab.Instance)
			{
				DestroyImmediate(prefab.Instance);
				needReApply = true;
			}
 
			if (needReApply)
			{
				PrefabUtility.ReplacePrefab(root, PrefabUtility.GetCorrespondingObjectFromSource(root), ReplacePrefabOptions.ConnectToPrefab);
			}
		}
	}
}
