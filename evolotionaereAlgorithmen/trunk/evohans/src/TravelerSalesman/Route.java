package TravelerSalesman;

public class Route {
	int anz;
	int[] indizes = new int[5];
	int j = 0;
	
	public Route() {
		for (int i=0; i<=5; i++){
			indizes[i]=0;
		}
	}
	void initialisiere(int knotenZahl) {
		int i = 0;
		System.out.println("Baue Route");
		indizes = new int[knotenZahl];
		for(i=0; i<knotenZahl; i++) {
			indizes[i] = i + 1;
		}
		
		if (j>=knotenZahl) {
			j = 0;
		}
		else if (j>0) {
			i = indizes[0];
			indizes[0] = indizes[j];
			indizes[j] = i;
		}
		j = j++;
		System.out.println(j);
	}
}
