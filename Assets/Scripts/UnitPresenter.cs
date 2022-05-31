using System.Linq;
using CoreMechanics;
using UnityEngine;

public class UnitPresenter : MonoBehaviour
{
	public GameObject m_UnitPrefab;
	public GameObject m_AttackCellPrefab;

	private GameObject mUnit;
	private GameObject[] mAttackCells;

	public void Present(Unit unit)
	{
		name = $"{unit.Type} presenter";
		if (mUnit == null)
		{
			m_UnitPrefab.SetActive(true);
			mUnit = Instantiate(m_UnitPrefab, transform);
			m_UnitPrefab.SetActive(false);
		}
		mUnit.transform.position = Vector2IntToVector3(unit.Position);

		if (mAttackCells == null || mAttackCells.Length < 9 || mAttackCells.Any(go => go == null))
		{
			mAttackCells = new GameObject[9];
			m_AttackCellPrefab.SetActive(true);
			for (var i = 0; i < 9; i++) mAttackCells[i] = Instantiate(m_AttackCellPrefab, transform);
			m_AttackCellPrefab.SetActive(false);
		}
		for (var i = 0; i < 9; i++) mAttackCells[i].transform.position = Vector2IntToVector3(unit.AttackPositions[i]);

		Vector3 Vector2IntToVector3(Vector2Int vec) => new(vec.x, 0, vec.y);
	}
}