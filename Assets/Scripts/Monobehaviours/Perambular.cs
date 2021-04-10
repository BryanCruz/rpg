using System.Collections;
using UnityEngine;

[RequireComponent( typeof( Rigidbody2D ) )]
[RequireComponent( typeof( CircleCollider2D ) )]
[RequireComponent( typeof( Animator ) )]

public class Perambular : MonoBehaviour
{
	public float velocidadePerseguicao; // velocidade do "inimigo" na área de spot
	public float velocidadePerambular;  // velocidade do "inimigo" passeando
	float velocidadeCorrente;           // velocidade do "inimigo" atribuida

	public float intervaloMudancaDirecao; // tempo para alterar direção
	public bool perseguePlayer; // indicador de perseguidor ou não

	Coroutine MoverCoroutine;

	Rigidbody2D rb2D;       // armazena o componente rigidbody 2d
	Animator animator;      // armazena o componente animator


	Transform alvoTransform = null;     // armazena o componente Transform do Alvo

	Vector3 posicaoFinal;
	float anguloAtual = 0;  // angulo atribuido

	CircleCollider2D circleCollider; // armazena o componente de Spot

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		velocidadeCorrente = velocidadePerambular;
		rb2D = GetComponent<Rigidbody2D>();
		StartCoroutine( rotinaPerambular() );
		circleCollider = GetComponent<CircleCollider2D>();
	}

	private void OnDrawGizmos()
	{
		if ( circleCollider != null )
		{
			Gizmos.DrawWireSphere( transform.position, circleCollider.radius );
		}
	}

	public IEnumerator rotinaPerambular()
	{
		while ( true )
		{
			EscolheNovoPontoFinal();

			if ( MoverCoroutine != null )
			{
				StopCoroutine( MoverCoroutine );
			}

			MoverCoroutine = StartCoroutine( Mover( rb2D, velocidadeCorrente ) );

			yield return new WaitForSeconds( intervaloMudancaDirecao );
		}
	}

	void EscolheNovoPontoFinal()
	{
		anguloAtual += Random.Range( 0, 360 );
		anguloAtual = Mathf.Repeat( anguloAtual, 360 );

		posicaoFinal += Vector3ParaAngulo( anguloAtual );
	}

	Vector3 Vector3ParaAngulo(float anguloEntradaGraus)
	{
		float anguloEntradaRadianos = anguloEntradaGraus * Mathf.Deg2Rad;

		return new Vector3( Mathf.Cos( anguloEntradaRadianos ), Mathf.Sin( anguloEntradaRadianos ), 0 );
	}

	public IEnumerator Mover(Rigidbody2D rbParaMover, float velocidade)
	{
		float distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;

		while ( distanciaFaltante > float.Epsilon )
		{
			if ( alvoTransform != null )
			{
				posicaoFinal = alvoTransform.position;
			}

			if ( rbParaMover != null )
			{
				animator.SetBool( "Caminhando", true );
				Vector3 novaPosicao = Vector3.MoveTowards( rbParaMover.position, posicaoFinal, velocidade * Time.deltaTime );
				rb2D.MovePosition( novaPosicao );
				distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
			}

			yield return new WaitForFixedUpdate();
		}

		animator.SetBool( "Caminhando", false );
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.gameObject.CompareTag( "Player" ) && perseguePlayer )
		{
			velocidadeCorrente = velocidadePerseguicao;
			alvoTransform = collision.gameObject.transform;
		}

		if ( MoverCoroutine != null )
		{
			StopCoroutine( MoverCoroutine );
		}

		MoverCoroutine = StartCoroutine( Mover( rb2D, velocidadeCorrente ) );
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ( collision.gameObject.CompareTag( "Player" ) )
		{
			animator.SetBool( "Caminhando", false );
			velocidadeCorrente = velocidadePerambular;

			if ( MoverCoroutine != null )
			{
				StopCoroutine( MoverCoroutine );
			}
			alvoTransform = null;
		}
	}

	void Update()
	{
		Debug.DrawLine( rb2D.position, posicaoFinal, Color.red );
	}
}
