using System.Collections.Generic;

/* Stores a list of levels. It is intended to be filled in by a .json */
[System.Serializable]
public class LevelList
{
	public List<Level> Level = new List<Level>();
}
