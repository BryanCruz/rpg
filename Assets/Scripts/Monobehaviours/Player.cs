using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Caractere
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.gameObject.CompareTag( "Coletavel" ) )
		{
			Item danoObjeto = collision.gameObject.GetComponent<Consumable>().item;

			if ( danoObjeto != null )
			{
				print( "o/a: " + danoObjeto.NomeObjeto );

				switch ( danoObjeto.tipoItem )
				{
					case Item.TipoItem.MOEDA:
						break;
					case Item.TipoItem.HEALTH:
						AjusteDanoObjeto( danoObjeto.quantidade );
						break;
					default:
						break;
				}

				collision.gameObject.SetActive( false );
			}
		}
	}

	public void AjusteDanoObjeto(int quantidade)
	{
		PontosDano = PontosDano + quantidade;
		print( "Ajuste por: " + quantidade + ". Novo valor = " + PontosDano );
	}
}
