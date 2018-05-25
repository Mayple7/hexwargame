using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Terrain
{
	Plains,
	Mountain,
	Water,
	City,

	NumTerrains
}

[Serializable]
public class HexData
{
	public int column;
	public int row;
	public int sum;

	public Terrain terrain;
}

public class Hex
{
	// column + row + sum = 0
	// Therefore column + row = -sum
	HexData hexData;

	static readonly float WidthMultiplier = Mathf.Sqrt(3) / 2f;

	public Hex(int c, int r)
	{
		hexData = new HexData();
		hexData.column = c;
		hexData.row = r;
		hexData.sum = -(c + r);

		hexData.terrain = Terrain.Plains;
	}

	public Vector3 Position()
	{
		float radius = 1f;
		float height = radius * 2f;
		float width = WidthMultiplier * height;
		float xPosition = width * (hexData.column + hexData.row / 2f);
		float rowOffset = width * (hexData.row / 2);
		
		return new Vector3(xPosition, 0, height * 0.75f * hexData.row);
	}

	public void SetTerrain(Terrain t)
	{
		hexData.terrain = t;
	}

	public HexData GetHexData()
	{
		return hexData;
	}
}
