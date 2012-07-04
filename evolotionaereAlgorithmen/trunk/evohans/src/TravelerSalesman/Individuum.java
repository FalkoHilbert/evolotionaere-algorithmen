package TravelerSalesman;
import java.util.ArrayList;
import java.util.TreeMap;


public class Individuum {
	public ArrayList<Integer> Gen = new ArrayList<Integer>();
	
	public TreeMap<Integer, ArrayList<Integer>> Allelnachbarn = new TreeMap<Integer, ArrayList<Integer>>();
	
	public void buildNachbarn() {
		
		for (int i = 0; i < this.Gen.size(); i++) {
			Allelnachbarn.put(this.Gen.get(i), sucheNachbarn(i));
		}
	}
	
	public ArrayList<Integer> sucheNachbarn(int index) {
		ArrayList<Integer> Nachbarn = new ArrayList<Integer>();
		
		Nachbarn.add(this.Gen.get((index + 1 + (this.Gen.size()-2)) % this.Gen.size()));
        Nachbarn.add(this.Gen.get((index + 1) % Gen.size()));

		return Nachbarn;
	}
}
