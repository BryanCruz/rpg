using UnityEngine;

[CreateAssetMenu( menuName = "Item" )]

public class Item : ScriptableObject
{
	public string NomeObjeto;
	public Sprite sprite;
	public int quantidade;
	public bool empilhavel;

	public enum TipoItem
	{
		MOEDA,
		MOEDAAZUL,
		HEALTH,
		CHOCOLATE,
		VARINHA,
		VARINHA2
	}

	public TipoItem tipoItem;
}
