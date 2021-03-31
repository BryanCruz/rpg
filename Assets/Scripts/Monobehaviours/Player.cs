using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Caractere
{
	public HealthBar healthBarPrefab; // referncia ao objeto prefab criado do HealthBar
	HealthBar healthBar;

	private void Start()
	{
		healthBar.caractere = this;
		pontosDano.valor = inicioPontosDano;
		healthBar = Instantiate( healthBarPrefab );
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.gameObject.CompareTag( "Coletavel" ) )
		{
			Item danoObjeto = collision.gameObject.GetComponent<Consumable>().item;

			if ( danoObjeto != null )
			{
				bool DeveDesaparecer = false;
				// print( "o/a: " + danoObjeto.NomeObjeto );

				switch ( danoObjeto.tipoItem )
				{
					case Item.TipoItem.MOEDA:
						DeveDesaparecer = true;
						break;
					case Item.TipoItem.HEALTH:
						DeveDesaparecer = AjusteDanoObjeto( danoObjeto.quantidade );
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
