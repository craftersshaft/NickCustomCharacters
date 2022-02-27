using Nick;
using System.Collections;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace CustomCharacterLoader.CustomCalls
{
	public class ActivateByPlayerIDCC : CustomCallMB
	{

		private string[] idarr;

		[SerializeField]
		public ActivateByPlayerArray[] objectsToActivate;

		public override string[] GetIdArray()
		{
			if (idarr == null)
			{
				idarr = new string[1] { "stage_activateByPlayer" };
			}
			return idarr;
		}

		public override void CallIndex(int i, GameAgent agent, GameInstance context)
		{
			if (i == 0)
			{
				ActivateByPlayer(agent, context);
			}
		}

		private void ActivateByPlayer(GameAgent agent, GameInstance context)
		{
			int copiedAgents = context.CopiedAgents;
			for (int j = 0; j < copiedAgents; j++)
			{
				for (int objection = 0; objection < objectsToActivate.Length; objection++)
				{
					if (context.GetAgent(j).GameUniqueIdentifier == objectsToActivate[objection].characterId)
					{
						if (context.GetAgent(j).playerIndex == objectsToActivate[objection].playerIndex && objectsToActivate[objection].usesPlayerIndex)
						{
							objectsToActivate[objection].objectToActivate.SetActive(true);
						}
						if (!objectsToActivate[objection].usesPlayerIndex)
						{
							objectsToActivate[objection].objectToActivate.SetActive(true);
						}
					}
				}
			}
		}
	}

	[System.Serializable]
	public class ActivateByPlayerArray
	{
		public GameObject objectToActivate;
		public string characterId;
		public bool usesPlayerIndex;
		public int playerIndex;
	}

}
