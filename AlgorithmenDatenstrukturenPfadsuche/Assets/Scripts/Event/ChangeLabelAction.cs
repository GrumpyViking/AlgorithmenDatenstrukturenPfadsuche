using System;
using Model;
using System.Collections;
using UnityEngine;

namespace Event
{
	public class ChangeLabelAction : BasicAction
	{
		private IDataObject[] os;
		private string txt;

		public ChangeLabelAction (string txt, params IDataObject[] os)
		{
			this.os = os;
			this.txt = txt;
		}


		public override IEnumerator Run ()
		{
			foreach (IDataObject x in os) {
				GameObject go = mapper.resolve (x);
				go.GetComponentInChildren<TextMesh> ().text = txt;
			}
			yield break;
		}
	}
}

