using System.Collections.Generic;
using UnityEngine;

public class Armas : MonoBehaviour
{
	public GameObject municaoPrefab; // armazena o prefab da Munição
	static List<GameObject> municaoPool; // pool de munição
	public int tamanhoPool; // tamanho da pool
	public float velocidadeArma; // velocidade da munição

	public void Awake()
	{
		if ( municaoPool == null )
		{
			municaoPool = new List<GameObject>();
		}

		for ( int i = 0; i < tamanhoPool; i++ )
		{
			GameObject municaoO = Instantiate( municaoPrefab );
			municaoO.SetActive( false );
			municaoPool.Add( municaoO );
		}
	}

	private void Update()
	{
		if ( Input.GetMouseButtonDown( 0 ) )
		{
			DisparaMunicao();
		}
	}

	public GameObject SpawnMunicao(Vector3 posicao)
	{
		foreach ( GameObject municao in municaoPool )
		{
			if ( municao.activeSelf == false )
			{
				municao.SetActive( true );
				municao.transform.position = posicao;

				return municao;
			}
		}

		return null;
	}

	void DisparaMunicao()
	{
		Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		GameObject municao = SpawnMunicao( transform.position );

		if ( municao != null )
		{
			Arco arcoScript = municao.GetComponent<Arco>();
			float duracaoTrajetoria = 1.0f / velocidadeArma;

			StartCoroutine( arcoScript.arcoTrajetoria( posicaoMouse, duracaoTrajetoria ) );
		}
	}

	private void OnDestroy()
	{
		municaoPool = null;
	}
}
