using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public class ObjectMapper
	{
		//Singleton
		private static ObjectMapper instance = new ObjectMapper();
		public static ObjectMapper Instance {
			get{
				return instance;
			}
		}

		//Normal class

		private IDictionary<IDataObject,GameObject> dict;

		private ObjectMapper ()
		{
			dict = new Dictionary<IDataObject,GameObject> ();
		}

		public void register(IDataObject o, GameObject go)
		{
			dict.Add (o, go);
		}

		public GameObject resolve(IDataObject o)
		{ 
			return dict [o];
		}
	}
}

