package TravelerSalesman;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.Random;

import org.omg.CORBA.portable.IndirectionException;



public class Population {
	int anzahl;
	Route[] r;
	
	
	public Population(int anzIndividuen, int anzKnoten) {
		int i;
		
		//anzahl = anzIndividuen;
		
		ArrayList<Individuum> travelers = new ArrayList<Individuum>();
		
		/*
		 * Generiere zuf�llige Routen f�r die Individuuen
		 */
		int[][] routen = new int[anzIndividuen][anzKnoten];
		int[][] kosten = new int[][] { 
			{  0,  5,  8, 11,  4,  7 }, 
			{  5,  0, 10,  4,  9, 12 },
			{  8, 10,  0,  6, 17,  8 }, 
			{ 11,  4,  6,  0,  6,  5 },
			{  4,  9, 17,  6,  0, 11 }, 
			{  7, 12,  8,  5, 11,  0 } };
		
		
		
		ArrayList<Integer> rand = new ArrayList<Integer>();
		ArrayList<Integer> pfad1  = new ArrayList<Integer>();
		ArrayList<ArrayList<Integer>> pfade = new ArrayList<ArrayList<Integer>>();
		
		ArrayList<ArrayList<Integer>> kinder = new ArrayList<ArrayList<Integer>>();
		
		for (i=1; i<=anzKnoten; i++) {
			rand.add(i);
		}
		
		for (int j = 0; j < anzIndividuen; j++) {
			
			
			Collections.shuffle(rand);
			
			for (i=0; i<=anzKnoten-1; i++) {
				routen[j][i] = rand.get(i);
			}
			Individuum traveler = new Individuum();
			traveler.Gen = rand;
			traveler.buildNachbarn();
			
			travelers.add(traveler);
//			pfade.add(rand);

			System.out.println("Individuum " + j + " Pfad " + traveler.Gen);
		}
		
		/*
		 * Generiere Kinder
		 * AdjList wird irgendwie immer gelöscht, daher wird sie immer neu erstellt
		 */

		for(int pop=0; pop <= 10; pop ++) {
			
			/*
			 * Baue Nachbarschaftsmatrix auf
			 */
			//ArrayList<ArrayList<Integer>> adjListe = new ArrayList<ArrayList<Integer>>();
			//ArrayList<Integer> indi = new ArrayList<Integer>();
//			
//			for (i = 0; i < anzKnoten; i++) {
//				int j=0;
				//Errechne Nachbarn
				
				/*
				 * Suche Startpunkt
				 * Der Startpunkt muss in beiden Listen bei der gleiche Zahl sein
				 */
//				for (int x = 0; x < routen[1].length; x++) {
//					if (routen[1][x] == routen[0][i]) {
//						j = x;
//					}
//				}
//				
				
//				//Baue Adjazenzmatrix auf = Population mit zufälliger Rundreise
			
//				int item1 = (i+1+(anzKnoten-2))%anzKnoten;
//				int item2 = ((i+1)%anzKnoten);
//				int item3 = (j+1+(anzKnoten-2))%anzKnoten;
//				int item4 = ((j+1)%anzKnoten);
//				
//				indi.add(routen[0][item1]);
//				indi.add(routen[0][item2]);
//				// Filtere unn�tige Wege heraus
//				if (indi.get(0) != routen[1][item3] && indi.get(1) != routen[1][item3] ) {
//					indi.add(routen[1][item3]);
//				}
//				if (indi.get(0) != routen[1][item4] && indi.get(1) != routen[1][item4]) {
//					indi.add(routen[1][item4]);
//				}
				
//				adjListe.add(indi);
//				indi = new ArrayList<Integer>();
//			}
			System.out.println("Adjazenzmatrix");
			System.out.println(adjListe.toString());
	
			
			//ArrayList<ArrayList<Integer>> Kind = new ArrayList<ArraysList<Integer>>();
			
	//		System.out.println(pfad1.get(0).toString());
	//		System.out.println(adjListe.get(0).get(0).toString());
	//		System.out.println(pfad1.indexOf(adjListe.get(0).get(0)));
			//System.out.println(adjListe.get(0).get(pfad1.indexOf(adjListe.get(0).get(0))));
			
			/*
			 * Erzeuge Kinder
			 * Suche Pfade mit den wenigsten Nachfolgern
			 * Entscheide per Zufall, falls gleich viele Nachfolger
			 */
			//[...]
			ArrayList<ArrayList<Integer>> tempAdj = new ArrayList<ArrayList<Integer>>();
			tempAdj = adjListe;

			ArrayList<Integer> tempPfad = new ArrayList<Integer>();
			ArrayList<Integer> nextPositions = new ArrayList<Integer>();
			
			addToPfad(tempAdj, tempPfad, pfade.get(0).get(0));
			int letztePos = 0; 
		
		
			while (tempPfad.size() < pfad1.size()) {
				
				letztePos = tempPfad.get(tempPfad.size()-1);
				int minSucc = 0;
				int minAnz = -1;
				nextPositions = new ArrayList<Integer>();
				for (Integer b : tempAdj.get(pfade.get(0).indexOf(letztePos))) {
//				for (Integer b : tempAdj.get(pfad1.indexOf(letztePos))) {
					
					int anz = tempAdj.get(pfade.get(0).indexOf(b)).size();
//					int anz = tempAdj.get(pfad1.indexOf(b)).size();
					System.out.println("Betrachte nächsten Knoten im Pfad");
					System.out.println("Nachbarn für Knoten " + b +" Anzahl: " +anz);
					
					if (minAnz == -1 || anz < minAnz) {
						minAnz = anz;
						minSucc = b;
						if (nextPositions.size() > 0) {
							nextPositions.remove(nextPositions.size()-1);
							nextPositions.add(b);
						} else {
							nextPositions.add(b);
						}
					} else if( anz == minAnz )
					{
						nextPositions.add(b);
					}
				};
				
				System.out.println("Menge der Nachfolger: " + nextPositions);
				Random generator = new Random();
				int roll = generator.nextInt(nextPositions.size());
	//			System.out.println("Zufällig folgender Nachfolger: " + roll);
	//			System.out.println("##########");
	//			System.out.println("Auswertung");
	//			System.out.println("Nachfolger mit den geringsten Nachbar ist die: "+minSucc);
	//			System.out.println("Nachfolger hat "+minAnz +" Nachbarn");
	//			System.out.println("##########");
				addToPfad(tempAdj, tempPfad, nextPositions.get(roll));
			}
			kinder.add(tempPfad);
			System.out.println(kinder);
		}
		
	}
	public static void addToPfad(ArrayList<ArrayList<Integer>> adjListeCopy, ArrayList<Integer> childList, int ele) {
		for (ArrayList<Integer> arrayList : adjListeCopy) {
			ArrayList<Integer> itemsToRemove = new ArrayList<Integer>();
			for (int integer : arrayList) {
				if (integer == ele) {
					itemsToRemove.add(arrayList.indexOf(integer));
					//Beende for, da Element gefunden
					integer = ele;
				}
			}
//			System.out.println(ele + " muss entfernt werden");
//			System.out.println("In " + arrayList + " wird die Position: "  +itemsToRemove + " entfernt");
			for (int integer : itemsToRemove) {
				//System.out.println("Eintrag wird entfern: "+itemsToRemove);
				arrayList.remove(integer);
			}
			
		}
		childList.add(ele);
		System.out.println("###############################");
		System.out.println("Neuer Inhalt kopierter AdjListe");
		System.out.println(adjListeCopy.toString());
		System.out.println("Neues Kind " + childList);
		System.out.println("###############################");
	}
}
