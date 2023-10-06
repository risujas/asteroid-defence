using System;
using UnityEngine;
using UnityEngine.Events;

public class GenericScriptCaller : MonoBehaviour
{
	[Serializable] public class GenericScriptCallerEvent : UnityEvent { }
	[SerializeField] private GenericScriptCallerEvent OnStart;
	[SerializeField] private GenericScriptCallerEvent OnUpdate;

	private void Start()
	{
		OnStart.Invoke();
	}

	private void Update()
	{
		OnUpdate.Invoke();
	}
}
