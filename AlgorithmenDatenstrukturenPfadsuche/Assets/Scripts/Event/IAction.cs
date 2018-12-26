using System;
using System.Collections;

namespace Event
{
	public interface IAction
	{
		IEnumerator Run();
	}
}

