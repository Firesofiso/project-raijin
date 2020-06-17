using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
	public List<PlayerController> players; 
	public List<GameObject> healthUI;
	public GameObject healthPoint, manaPoint, emptyPoint;

	public Image canAttackImage;

    // Start is called before the first frame update
    void Start()
    {
		GameObject[] go_Players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject go in go_Players) {
			players.Add(go.GetComponent<PlayerController>());
		}
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < players.Count; i++) {
			canAttackImage.color = players[i]._canAttack ? Color.green : Color.red;
			UpdateHealthBar(i);
		}
    }

	private void UpdateHealthBar(int index) {
		foreach (Transform child in healthUI[index].transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		int playerMaxStat = players[index].maxHealth > players[index].maxMana ? players[index].maxHealth : players[index].maxMana;
		Vector3 lastPos = new Vector3(0, 0, 0);
		int xIncrement = 3;
		int playerCurHealth = players[index].curHealth;
		int playerCurMana = players[index].curMana;


		for (int i = 0; i < playerMaxStat; i++)
		{
			GameObject newEmptyContainer = Instantiate(emptyPoint, lastPos, Quaternion.identity, healthUI[index].transform);
			newEmptyContainer.transform.SetParent(healthUI[index].transform);
			newEmptyContainer.transform.localPosition = lastPos;
			lastPos.x += xIncrement;
		}
		lastPos = new Vector3(0, 0, 0);
		for (int i = 0; i < playerCurHealth; i++)
		{
			GameObject newFilledContainer = Instantiate(healthPoint, lastPos, Quaternion.identity, healthUI[index].transform);
			newFilledContainer.transform.SetParent(healthUI[index].transform);
			newFilledContainer.transform.localPosition = lastPos;
			lastPos.x += xIncrement;
		}
		lastPos = new Vector3(0, 0, 0);
		for (int i = 0; i < playerCurMana; i++)
		{
			GameObject newFilledContainer = Instantiate(manaPoint, lastPos, Quaternion.identity, healthUI[index].transform);
			newFilledContainer.transform.SetParent(healthUI[index].transform);
			newFilledContainer.transform.localPosition = lastPos;
			lastPos.x += xIncrement;
		}
	}
}
