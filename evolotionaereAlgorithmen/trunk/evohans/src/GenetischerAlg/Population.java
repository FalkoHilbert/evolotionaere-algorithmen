package GenetischerAlg;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Random;

public class Population {
	ArrayList<Individuum> pop = new ArrayList<Individuum>();
	ArrayList<Individuum> popTemp = new ArrayList<Individuum>();
	ArrayList<Individuum> childPop = new ArrayList<Individuum>();
	
	public Population(int resultsExp, double oI, double uI) {
		Individuum indi;
		for (int i = 0; i < 10; i++) {
			indi = new Individuum(resultsExp, oI, uI);
			pop.add(indi);
		}
		System.out.println("Population erzeugt");
	}
	
	public void rekombiniere2Punkt() {
		Random rand = new Random();
		popTemp = pop;
		Collections.shuffle(popTemp);
		for (int i = 0; i < popTemp.size()-1; i++) {
			
			Individuum par1 = new Indi
			childPop.add(e);
		}
	}

}
