using System;
using Model;
using UnityEngine;
using System.Collections;

namespace Event
{
	public class BlinkAction : BasicAction
	{
		private IDataObject[] os;
		private Color col;
		private int cnt;
		private float duration;

		public BlinkAction (Color col, int cnt, float duration, params IDataObject[] os)
		{
			this.os = os;
			this.col = col;
			this.cnt = cnt;
			this.duration = duration;
		}

		public override IEnumerator Run ()
		{
			GameObject[] gos = new GameObject[os.Length];
			Color[] origs = new Color[os.Length];

			for (int i=0; i<os.Length; i++) {
				gos[i] = mapper.resolve (os[i]);
				origs[i] = gos[i].GetComponentInChildren<Renderer> ().material.GetColor ("_Color");
			}

			for (int j=0; j<cnt; j++) {
				for (int i=0; i<os.Length; i++) {
					gos[i].GetComponentInChildren<Renderer> ().material.SetColor ("_Color", col);
				}
				yield return new WaitForSeconds (duration);
				for (int i=0; i<os.Length; i++) {
					gos[i].GetComponentInChildren<Renderer> ().material.SetColor ("_Color", origs[i]);
				}
				if (j != os.Length - 1) {
					yield return new WaitForSeconds (duration);
				}
			}
		}

	}
}

