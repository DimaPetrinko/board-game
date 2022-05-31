using System.Collections.Generic;
using System.Linq;
using CoreMechanics;
using UnityEngine;

public class Test : MonoBehaviour
{
	[Header("First unit")]
	public bool m_FirstAttacks;
	public UnitConfig m_Config1;
	public Vector2Int m_Position1;
	public Orientation m_Orientation1;

	[Header("Second unit")]
	public UnitConfig m_Config2;
	public Vector2Int m_Position2;
	public Orientation m_Orientation2;

	[Header("Presenting")]
	public UnitPresenter m_UnitPresenterPrefab;

	private readonly List<UnitPresenter> mUnitPresenters = new();

	private void OnValidate()
	{
		var u1 = new Unit(m_Config1);
		u1.Position = m_Position1;
		u1.Orientation = m_Orientation1;

		var u2 = new Unit(m_Config2);
		u2.Position = m_Position2;
		u2.Orientation = m_Orientation2;

		var attackHandler = new AttackHandler();
		if (m_FirstAttacks) attackHandler.ResolveClash(u1, u2);
		else attackHandler.ResolveClash(u2, u1);

		Debug.Log($"First hp {u1.Health}, Second hp {u2.Health}");
		if (u1.Dead) Debug.LogError("First is Dead!");
		if (u2.Dead) Debug.LogError("Second is Dead!");

		PresentUnits(new[] {u1, u2});
	}

	private void PresentUnits(IList<Unit> units)
	{
		var count = Mathf.Max(mUnitPresenters.Count, units.Count);

		var presentersOnScene = FindObjectsOfType<UnitPresenter>()
			.Where(p => p != m_UnitPresenterPrefab && !mUnitPresenters.Contains(p))
			.ToList();
		if (presentersOnScene.Count > 0) mUnitPresenters.AddRange(presentersOnScene);
		mUnitPresenters.Where(p => p == null).ToList().ForEach(p => mUnitPresenters.Remove(p));
		for (var i = 0; i < count; i++)
		{
			if (i >= mUnitPresenters.Count)
			{
				m_UnitPresenterPrefab.gameObject.SetActive(true);
				mUnitPresenters.Add(Instantiate(m_UnitPresenterPrefab));
				m_UnitPresenterPrefab.gameObject.SetActive(false);
			}

			if (i < units.Count)
			{
				mUnitPresenters[i].gameObject.SetActive(true);
				mUnitPresenters[i].Present(units[i]);
			}
			else mUnitPresenters[i].gameObject.SetActive(false);
		}
	}
}
