using UnityEngine;
using myengine.BlockPuzzle;
namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "TileObjectGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "",
	    order = 120)]
	public sealed class TileObjectGameEvent : GameEventBase<TileObject>
	{
	}
}