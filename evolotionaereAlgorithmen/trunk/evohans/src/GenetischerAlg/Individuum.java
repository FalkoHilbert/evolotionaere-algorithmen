package GenetischerAlg;

import java.util.ArrayList;

public class Individuum {
	public Gen indiGen;


	
	public Individuum(int resultsExp, double oI, double uI) {
		/*
		 * Eine Sequenzlänge beträgt 3 - ist je ein Genabschnitt
		 * Die Genlänge (Summe der Sequenzen) beträgt 4
		 */

		indiGen = new Gen(resultsExp, 3, oI,uI);
		
	}
	
	
	
}
