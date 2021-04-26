using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Caractere
{
	public Inventario inventarioPrefab; //referencia ao objeto prefabc criando do inventario
	Inventario inventario;

	public HealthBar healthBarPrefab; // referncia ao objeto prefab criado do HealthBar
	HealthBar healthBar;

	public PontosDano pontosDano; // tem o valor de "saúde" do objeto

	bool showHelp = false; // checa se é pra mostrar o texto de ajuda

	private void Start()
	{
		inventario = Instantiate( inventarioPrefab );
		pontosDano.valor = inicioPontosDano;
		healthBar = Instantiate( healthBarPrefab );
		healthBar.caractere = this;
	}

	public override IEnumerator DanoCaractere(int dano, float intervalo)
	{
		while ( true )
		{
			StartCoroutine( FlickerCaractere() );

			pontosDano.valor -= dano;

			if ( pontosDano.valor <= float.Epsilon )
			{
				KillCaractere();
				SceneManager.LoadScene( "GameOver" );
				break;
			}

			if ( intervalo > float.Epsilon )
			{
				yield return new WaitForSeconds( intervalo );
			}
			else
			{
				break;
			}
		}
	}

	public override void KillCaractere()
	{
		base.KillCaractere();
		Destroy( healthBar.gameObject );
		Destroy( inventario.gameObject );
	}

	public override void ResetCaractere()
	{
		inventario = Instantiate( inventarioPrefab );
		healthBar = Instantiate( healthBarPrefab );
		healthBar.caractere = this;
		pontosDano.valor = inicioPontosDano;
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
					// caso seja qualquer um dos seguintes itens, adiciona no inventario e remove o objeto da cena caso necessário
					case Item.TipoItem.MOEDA:
					case Item.TipoItem.MOEDAAZUL:
					case Item.TipoItem.CHOCOLATE:
					case Item.TipoItem.VARINHA:
					case Item.TipoItem.VARINHA2:
						// DeveDesaparecer = true;
						DeveDesaparecer = inventario.AddItem( DanoObjeto );
						break;

					// caso seja vida, cura o player e remove o objeto da cena caso necessário
					case Item.TipoItem.HEALTH:
						DeveDesaparecer = AjusteDanoObjeto( DanoObjeto.quantidade );
						break;

					// default, não faz nada
					default:
						break;
				}

				// se o item deve desaparecer, desativa ele
				if ( DeveDesaparecer )
				{
					collision.gameObject.SetActive( false );
				}


				// checa se o player pegou todos os coletaveis
				bool coletouTodosOsColetaveis = checaSeColetouTodosOsColetaveis();
				if ( coletouTodosOsColetaveis )
				{
					SceneManager.LoadScene( "MissaoCumprida" );
				}

			}
		}

		// mostra a mensagem de ajuda quando chega perto do anjo
		if ( collision.gameObject.CompareTag( "Helper" ) )
		{
			showHelp = true;
		}
	}

	// evento para exibir itens de interface de usuario
	private void OnGUI()
	{
		// define o conteudo da mensagem de ajuda
		string mensagemDeAjuda = "Olá aventureiro, você deve nos salvar!\nColete os seguintes itens para podermos sair desse lugar:\n" +
								 "- 5 moedas da cor azul\n- 5 moedas da cor amarela\n-1 barra de chocolate\n- 1 varinha perdida do mago Blah\n- 1 varinha perdida do feiticeiro Wadda\n" +
								 "Tome cuidado com os inimigos e boa sorte!";

		// exibe a mensagem caso necessário
		if ( showHelp ) GUI.Box( new Rect( 555, 555, 400, 130 ), mensagemDeAjuda );
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// deixa de mostrar a mensagem de ajuda quando anda pra longe do anjo
		if ( collision.gameObject.CompareTag( "Helper" ) )
		{
			showHelp = false;
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

	public bool checaSeColetouTodosOsColetaveis()
	{
		Item[] itemsDoPlayer = inventario.items;

		// checa se o player está com o inventário cheio
		foreach ( Item item in itemsDoPlayer )
		{
			if ( item == null )
			{
				return false;
			}
		}


		// variavel pra marcar os slots com qtd de itens completos do player
		bool[] completouSlot = { false, false, false, false, false };
		int numTipoDeMoedasColetadas = 0;

		// checa se pegou 5 de cada tipo de moeda
		for ( int i = 0; i < itemsDoPlayer.Length; i++ )
		{
			// se já usamos esse slot pra completar algum coletavel, passa pro próximo slot
			if ( completouSlot[i] )
			{
				continue;
			}


			// se coletou 5, signifca que era alguma moeda e a gente marca esse slot como completo
			if ( itemsDoPlayer[i].quantidade == 5 )
			{
				completouSlot[i] = true;
				numTipoDeMoedasColetadas += 1;
			}
		}

		// se ainda não coletou todas as moedas dos 2 tipos, retorna falso
		if ( numTipoDeMoedasColetadas < 2 )
		{
			return false;
		}

		// checa se pegou 1 de cada item especial
		for ( int i = 0; i < itemsDoPlayer.Length; i++ )
		{
			// se já usamos esse slot pra completar algum coletavel, passa pro próximo slot
			if ( completouSlot[i] )
			{
				continue;
			}


			// se coletou 1, signifca que era algum item especial, pois já contamos as moedas
			if ( itemsDoPlayer[i].quantidade == 1 )
			{
				completouSlot[i] = true;
			}
		}


		// checa se completou todos os slots
		for ( int i = 0; i < completouSlot.Length; i++ )
		{
			// caso seja false, signica que ainda falta algum item
			if ( completouSlot[i] == false )
			{
				return false;
			}
		}

		// se chegou até aqui, é porque coletou todos os coletaveis
		return true;
	}
}
