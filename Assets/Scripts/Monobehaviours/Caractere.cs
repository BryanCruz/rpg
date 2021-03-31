using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Caractere : MonoBehaviour
{
	// public int PontosDano;  // versão anterior do valor de "dano"
	public PontosDano pontosDano; // novo tipo que tem o valor do objeto script 

	// public int MaxPontosDano;	// versão anterior do valor máximo de "dano"
	public float inicioPontosDano; // valor mínimo inicial de "saúde" do Player
	public float MaxPontosDano;  // valor máximo permitido de "saúde" do Player
}
