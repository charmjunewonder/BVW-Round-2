using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class MapRepresentation : MonoBehaviour {

	Grid[] grids;
	public int mapWidth  = 40;
	public int mapHeight = 23;
	int width;
	int height;
	public GameObject cube;
	public GameObject ObstacleGroup;
	Grid[] passengerPlaces;

	public Grid christmasTree;
	public Grid mappleStore;
	public Grid nailSalon;
	public Grid holeFood;
	public Grid wineSpirits;
	public Grid stationery;
	private Grid[] interestPlaces;

	public TextAsset asset; // Assign that variable through inspector
	private string assetText;

	public int[,] locationMatrix;


	// Use this for initialization
	void Start () {
		assetText = asset.text;
		width = (int)renderer.bounds.size.x / Grid.GRID_SIZE;
		height = (int)renderer.bounds.size.z / Grid.GRID_SIZE;
		grids = new Grid[mapWidth * mapHeight];
		readMapFromFile ();
		passengerPlaces = new Grid[5];
		interestPlaces = new Grid[5];

		initializePassengerPlaces ();
		initializeInterestPlaces ();

		/*christmasTree = getGrid (33, 18);
		mappleStore = getGrid (6, 14);
		nailSalon = getGrid (36, 32);
		holeFood = getGrid (13, 25);
		wineSpirits = getGrid (33, 12);
		stationery = getGrid (52, 16);*/
		/*christmasTree = getGrid (20, 6);//
		mappleStore = getGrid (32, 13);//
		nailSalon = getGrid (30, 18);
		holeFood = getGrid (2, 14);//
		wineSpirits = getGrid (13, 14);//
		stationery = getGrid (5, 14);*/
		
		//generateRandomPlaceMatrix ();
	}

	private void initializeInterestPlaces(){
		christmasTree = getGrid (20, 4);
		interestPlaces [0] = christmasTree;
		
		mappleStore = getGrid (32, 13);//
		interestPlaces [1] = mappleStore;
		
		nailSalon = getGrid (4, 11);//
		interestPlaces [2] = nailSalon;
		
		holeFood = getGrid (2, 14);//
		interestPlaces [3] = holeFood;
		
		wineSpirits = getGrid (13, 14);//
		interestPlaces [4] = wineSpirits;
		
	}

	private void initializePassengerPlaces(){
		passengerPlaces[0] = getGrid (7, 0);
		passengerPlaces[1] = getGrid (0, 3);
		passengerPlaces[2] = getGrid (0, 12);
		passengerPlaces[3] = getGrid (0, 14);
		passengerPlaces[4] = getGrid (8, 22);
//		interestingPlaces[5] = getGrid (, );
//		interestingPlaces[6] = getGrid (, );
//		interestingPlaces[7] = getGrid (3, 17);
//		interestingPlaces[8] = getGrid (14, 18);
//		interestingPlaces[9] = getGrid (17, 17);

	}

	public int[,] generateRandomPlaceMatrix()
	{
		//five places 5 girls;
		int [,] templocationMatrix = new int[5, 5]{{0,0,0,0,0},
													{0,1,0,0,0},
													{0,0,2,0,0},
													{0,0,0,3,0},
													{0,1,2,3,4}};
		int [,] temp2locationMatrix = new int[5, 5];

		int[,] leftoverplaces = new int[4, 4]{ {1,2,3,4},
												{0,2,3,4},
												{0,1,3,4},
												{0,1,2,4}};

		bool[] done = new bool[5]{false,false,false,false,false};
		int[] randomorder = new int[5];
		//UnityEngine.Random.seed = Mathf.FloorToInt(Time.time);

		for (int i=0; i<4; i++) 
		{
			Array.Clear(done,0,5);
			for(int j=0;j<4;j++)
			{
				int pick = 0;
				do
					pick = UnityEngine.Random.Range(0,4);
				while(done[pick]);
				Debug.Log (pick);
				done[pick] = true;
				templocationMatrix[i,(i+1+j)%5] = leftoverplaces[i,pick];
			}
		}

		Array.Clear(done,0,5);

		for(int j=0;j<4;j++)
		{
			int pick = 0;
			do
				pick = UnityEngine.Random.Range(0,4);
			while(done[pick]);
			done[pick]=true;
			randomorder[j] = pick;
		}
		randomorder [4] = 4;

		for (int i=0; i<5; i++)
						for (int j=0; j<5; j++)
								temp2locationMatrix [randomorder [i], j] = templocationMatrix [i, j];

		Array.Clear(done,0,5);

		for(int j=0;j<5;j++)
		{
			int pick = 0;
			do
				pick = UnityEngine.Random.Range(0,5);
			while(done[pick]);
			done[pick]=true;
			randomorder[j] = pick;
		}

		for (int i=0; i<5; i++)
			for (int j=0; j<5; j++)
				templocationMatrix [j,randomorder [i]] = temp2locationMatrix [j,i];

		locationMatrix = templocationMatrix;

		string outt = "";
		for (int i=0; i<5; i++) {
						for (int j=0; j<5; j++)
								outt += locationMatrix [i, j] + " ";

			outt += "\n";
				}
				
		Debug.Log (outt);

		return locationMatrix;
	}

	public Grid getInterestPlace(int index){
		return interestPlaces[index];
	}

	private void passengersMove(){
		GameObject[] passengers = GameObject.FindGameObjectsWithTag ("Passenger");
		foreach (GameObject p in passengers) {
			p.GetComponent<Passenger>().setStartPosition(getRandomPlace());
			p.GetComponent<Passenger>().moveInRoutine();
		}
	}

	public Grid getRandomPlace(){
		return passengerPlaces [UnityEngine.Random.Range (0, passengerPlaces.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void findShortestPath (Person person, Grid endGrid){
		Grid startGrid = person.currentPosition;
		
		Queue fakeQueue = new Queue();
		fakeQueue.Enqueue (startGrid);
		
		for(int x = 0; x < mapWidth; ++x){
			for(int y = 0; y < mapHeight; ++y){
				Grid g = grids[x*mapHeight+y];
				g.gridState = Grid.GridState.NotProcessed;
				g.fromGrid = null;
			}
		}
		
		int totalQueueObjectNum = 0;
		startGrid.gridState = Grid.GridState.InCloseList;
		Grid queueObject = startGrid;
		totalQueueObjectNum++;
		ArrayList storeArray = new ArrayList ();
		while (queueObject != endGrid && fakeQueue.Count > 0) {
			
			queueObject = (Grid)fakeQueue.Dequeue();
			ArrayList adjacentGrids = getFourAdjacentGrid(queueObject);
			foreach (Grid adjacentPoint in adjacentGrids){
				if(!adjacentPoint) continue;
				if (adjacentPoint.gridState != Grid.GridState.InCloseList) {
					storeArray.Add( adjacentPoint);
					totalQueueObjectNum++;
					adjacentPoint.fromGrid = queueObject;
					adjacentPoint.gridState = Grid.GridState.InCloseList;
					//adjacentPoint.movementCost = queueObject.movementCost + 1;
				}
			}
			 	if (fakeQueue.Count == 0) {
					foreach(Grid g in storeArray)
						fakeQueue.Enqueue(g);
					storeArray.Clear();
			}
		}
		
		Grid fromNode = endGrid;
		Person p = person.GetComponent<Person> ();
		p.path.Clear();
		while (fromNode.fromGrid && fromNode.fromGrid != startGrid) {
			p.path.Push(fromNode);
			fromNode = fromNode.fromGrid;
		}
		p.path.Push(fromNode);

	}
	public ArrayList getFourAdjacentGrid(Grid currentGrid){
		ArrayList adjacentGrids = new ArrayList ();
		Vector2 currentLocation = currentGrid.pointOfGrid;
		for (int x = -1; x < 2; ++x){
			for (int y = -1; y < 2; ++y) {
				if (Mathf.Abs(x+y)==1) {
					Grid g = getGrid((int)currentLocation.x+x, (int)currentLocation.y+y);
					if (g && g.gridType != Grid.GridType.UnaccessableGrid) {
						adjacentGrids.Add(g);
					}
				}
			}
		}
		return adjacentGrids;
	}
	
	public Grid getGrid(int x, int y){
		if(!isInsideGrids(x, y)) return null;
		return grids [y * mapWidth + x];
	}

	public Grid getGrid(Vector2 position){
		if(!isInsideGrids((int)position.x, (int)position.y)) return null;
		return grids [(int)position.y * mapWidth + (int)position.x];
	}
	
	void readMapFromFile(){
		try
		{
			/*using (StreamReader sr = new StreamReader(assetText))
			{
				char[] b = new char[1];

				for (int y = 0; y < mapHeight; ++y) {
					StringReader lr = new StringReader(sr.ReadLine());
					for (int x = 0; x < mapWidth; ++x) {
						lr.Read(b, 0, 1);
						Grid g = ScriptableObject.CreateInstance<Grid>();
						g.setPointOfGrid(x, mapHeight - y - 1);
						grids[x+(mapHeight - y - 1)*mapWidth] = g;
						if (b[0] == '@') {
							g.gridType = Grid.GridType.UnaccessableGrid;
							GameObject cubeA = Instantiate(cube) as GameObject;
							cubeA.transform.position = new Vector3(g.pointOfGrid.x, 0.5f, g.pointOfGrid.y);
							cubeA.transform.parent = ObstacleGroup.transform;

						}
						else if (b[0] == '.'){
							g.gridType = Grid.GridType.NormalGrid;
						}
					}
				}

			}*/
			StringReader strReader = new StringReader(assetText);
			char[] b = new char[1];
			
			for (int y = 0; y < mapHeight; ++y) {
				string aLine = strReader.ReadLine();
				StringReader lr = new StringReader(aLine);

				for (int x = 0; x < mapWidth; ++x) {
					lr.Read(b, 0, 1);
					Grid g = ScriptableObject.CreateInstance<Grid>();
					g.setPointOfGrid(x, mapHeight - y - 1);
					grids[x+(mapHeight - y - 1)*mapWidth] = g;
					if (b[0] == '@') {
						g.gridType = Grid.GridType.UnaccessableGrid;
//						GameObject cubeA = Instantiate(cube) as GameObject;
//						cubeA.transform.position = new Vector3(g.pointOfGrid.x, 0.5f, g.pointOfGrid.y);
//						cubeA.transform.parent = ObstacleGroup.transform;
						
					}
					else if (b[0] == '.'){
						g.gridType = Grid.GridType.NormalGrid;
					}
				}
			}
		}
		catch (Exception e)
		{
			Debug.Log("The file could not be read:");
			Debug.Log(e.Message);
		}
	}

	bool isInsideGrids(int x, int y){
		return (x >= 0 && x < mapWidth) && (y >= 0 && y < mapHeight);
	}
}
