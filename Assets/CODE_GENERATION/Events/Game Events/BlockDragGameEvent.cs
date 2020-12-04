using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "BlockDragGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "BlockDrag",
	    order = 120)]
	public sealed class BlockDragGameEvent : GameEventBase<BlockDrag>
	{
	}
}