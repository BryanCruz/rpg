using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Caractere
{
	public Inventario inventarioPrefab; //referencia ao objeto prefabc criando do inventario
	Inventario inventario;

	public HealthBar healthBarPrefab; // referncia ao objeto prefab criado do HealthBar
	HealthBar healthBar;

	private void Start()
	{
		inventario = Instantiate( inventarioPrefab );
		pontosDano.valor = inicioPontosDano;
		healthBar = Instantiate( healthBarPrefab );
		healthBar.caractere = this;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.gameObject.CompareTag( "Coletavel" ) )
		{
			Item DanoObjeto = collision.gameObject.GetComponent<Consumable>().item;

			if ( DanoObjeto != null )
			{
				bool DeveDesaparecer = false;
				// print( "o/a: " + danoObjeto.NomeObjeto );

				switch ( DanoObjeto.tipoItem )
				{
					case Item.TipoItem.MOEDA:
						// DeveDesaparecer = true;
						DeveDesaparecer = inventario.AddItem( DanoObjeto );
						break;
					case Item.TipoItem.HEALTH:
						DeveDesaparecer = AjusteDanoObjeto( DanoObjeto.quantidade );
						break;
					default:
						break;
				}

				if ( DeveDesaparecer )
				{
					collision.gameObject.SetActive( false );
				}
			}
		}
	}

	public bool AjusteDanoObjeto(int quantidade)
	{
		if ( pontosDano.valor < MaxPontosDano )
		{
			pontosDano.valor = pontosDano.valor + quantidade;
			print( "Ajuste PD por: " + quantidade + ". Novo valor = " + pontosDano.valor );
			return true;
		}
		else return false;
	}
}
