package GenetischerAlg;

import java.util.ArrayList;
import java.util.Random;

public class Gen {
	public static int k;
	public static int genSize;
	public boolean[] gen;
	public ArrayList<Double> decGen;
	public static int results;
	
	public double oberesIntervall;
	public double unteresIntervall;
	
	public Gen(int resultsExp, int sizeK, double oI, double uI) {
		decGen = new ArrayList<Double>();
		k = sizeK;
		genSize = resultsExp*k;
		oberesIntervall = oI;
		unteresIntervall = uI;
		results = resultsExp;
		
		for (int i = 0; i < genSize; i++) {
			makeGen();
		}
		for (int i = 0; i < genSize; ) {
			decGen.add(calcDecGen(i));
			i=i+k;
		}
		System.out.print("Gen=\t");
		for (int i = 0; i < genSize; i++) {
			System.out.print((gen[i] ? 1.0 : 0.0) +" ");
		}
		System.out.println();
		System.out.println("dec(A)=\t"+decGen);
		
	}
	public void makeGen() {
		Random rand = new Random();
		gen = new boolean[results*k];
		for (int i = 0; i < genSize; i++) {
			gen[i] = rand.nextBoolean();
		}
		
	}
	
	public double calcDecGen(int abs) {
		double a = 0.0;
		double tempD = 0.0;
		a = oberesIntervall + (oberesIntervall-unteresIntervall)/(Math.pow(2,k)-1);
		System.out.println("Abstand " + abs);
		for (int i = abs; i < k+abs; i++) {
			tempD = tempD + (gen[k+abs-i-1] ? 1.0 : 0.0) * Math.pow(2, i);
		}
		a = a * tempD;
		return a;
	}
	public boolean[] retGenAbschnitt(int abs) {
		boolean[] ret = new boolean[genSize/2];
		switch (abs) {
		case 1:
			for (int i = 0; i < ret.length; i++) {
				ret[i] = gen[i];
			}
			break;

		case 2:
			for (int i = ret.length; i < gen.length; i++) {
				ret[i] = gen[i];
			}
			break;
		default:
			break;
		}
		
		return ret;
	}
}
