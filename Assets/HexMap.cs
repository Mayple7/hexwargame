using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Map Data for serialization.
[Serializable]
public class MapData
{
	public int mapSizeX;
	public int mapSizeY;
	public HexData[,] hexData;
	public string filename = "WarMap";
}

public class HexMap : MonoBehaviour
{
	public GameObject HexPrefab;

	public Material[] HexMaterials;

	int mapWidth = 30;
	int mapHeight = 20;

	public Hex[,] hexData;
	public GameObject[,] hexGameObjects;

	// Use this for initialization
	void Start ()
	{
		this.GenerateMap();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			this.SaveHexMap();
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			this.LoadHexMap();
		}
	}
	
	// Generates our hexes.
	public void GenerateMap()
	{
		hexData = new Hex[mapWidth, mapHeight];
		hexGameObjects = new GameObject[mapWidth, mapHeight];

		int widthStart = 0;
		for (int j = 0; j < mapHeight; ++j)
		{
			for (int i = widthStart; i < mapWidth + widthStart; ++i)
			{
				Hex h = new Hex(i, j);

				GameObject hex = Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);

				h.SetTerrain((Terrain)UnityEngine.Random.Range(0, (int)Terrain.NumTerrains));
				MeshRenderer mesh = hex.GetComponentInChildren<MeshRenderer>();
				mesh.material = HexMaterials[(int)h.GetHexData().terrain];

				hexData[i - widthStart, j] = h;
				hexGameObjects[i - widthStart, j] = hex;
			}

			if (j % 2 != 0)
			{
				widthStart -= 1;
			}
		}
	}

	public void SaveHexMap()
	{
		MapData saveData = new MapData();

		saveData.mapSizeX = mapWidth;
		saveData.mapSizeY = mapHeight;
		saveData.hexData = new HexData[mapWidth, mapHeight];

		int widthStart = 0;
		for (int j = 0; j < mapHeight; ++j)
		{
			for (int i = widthStart; i < mapWidth + widthStart; ++i)
			{
				saveData.hexData[i - widthStart, j] = this.hexData[i - widthStart, j].GetHexData();
			}

			if (j % 2 != 0)
			{
				widthStart -= 1;
			}
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file;

		string fullFilePath = Application.persistentDataPath + "/Maps/";

		if (!Directory.Exists(fullFilePath))
			Directory.CreateDirectory(fullFilePath);

		fullFilePath += saveData.filename + ".hexmap";

		if (fullFilePath.Length == 0)
			return;

		file = File.OpenWrite(fullFilePath);
		
		bf.Serialize(file, saveData);

		file.Close();
	}

	public void LoadHexMap()
	{
		foreach(GameObject hex in hexGameObjects)
		{
			DestroyImmediate(hex);
		}

		string fullFilePath = Application.dataPath + "/Maps/" + "WarMap" + ".hexmap";
		if (!File.Exists(fullFilePath))
		{
			fullFilePath = Application.persistentDataPath + "/Maps/" + "WarMap" + ".hexmap";
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.OpenRead(fullFilePath);

		MapData mapData = (MapData)bf.Deserialize(file);

		hexData = new Hex[mapWidth, mapHeight];
		hexGameObjects = new GameObject[mapWidth, mapHeight];

		int widthStart = 0;
		for (int j = 0; j < mapHeight; ++j)
		{
			for (int i = widthStart; i < mapWidth + widthStart; ++i)
			{
				HexData loadedHex = mapData.hexData[i - widthStart, j];
				Hex h = new Hex(loadedHex.column, loadedHex.row);
				h.SetTerrain(mapData.hexData[i - widthStart, j].terrain);

				GameObject hex = Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);

				MeshRenderer mesh = hex.GetComponentInChildren<MeshRenderer>();
				mesh.material = HexMaterials[(int)loadedHex.terrain];

				hexData[i - widthStart, j] = h;
				hexGameObjects[i - widthStart, j] = hex;
			}

			if (j % 2 != 0)
			{
				widthStart -= 1;
			}
		}

		file.Close();
	}
}
