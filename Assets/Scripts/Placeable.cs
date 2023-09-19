using System;
using UnityEngine;
using UnityEngine.Events;

public class Placeable : MonoBehaviour
{
	[SerializeField] protected float placementCost = 20.0f;
	[SerializeField] protected bool allowMultiPlacement = true;
	[SerializeField] protected bool addToAnchorHierarchy = true;
	[SerializeField] protected GameObject placementEffect = null;

	public float PlacementCost => placementCost;
	public bool AllowMultiPlacement => allowMultiPlacement;
	public bool AddToAnchorHierarchy => addToAnchorHierarchy;
	public GameObject PlacementEffect => placementEffect;

	[Serializable] public class PlacementEvent : UnityEvent { }
	public PlacementEvent placementEvent;
}
