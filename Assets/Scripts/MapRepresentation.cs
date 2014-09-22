using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class MapRepresentation : MonoBehaviour {

	Grid[] grids;
	int mapWidth  = 40;
	int mapHeight = 23;
	int width;
	int height;
	public GameObject cube;
	public GameObject ObstacleGroup;
	Grid[] interestingPlaces;

	public Grid christmasTree;
	public Grid mappleStore;
	public Grid nailSalon;
	public Grid holeFood;
	public Grid wineSpirits;
	public Grid stationery;

	// Use this for initialization
	void Start () {
		width = (int)renderer.bounds.size.x / Grid.GRID_SIZE;
		height = (int)renderer.bounds.size.z / Grid.GRID_SIZE;
		grids = new Grid[mapWidth * mapHeight];
		readMapFromFile ();
		interestingPlaces = new Grid[5];
		initializeInterestPlaces ();

		/*christmasTree = getGrid (33, 18);
		mappleStore = getGrid (6, 14);
		nailSalon = getGrid (36, 32);
		holeFood = getGrid (13, 25);
		wineSpirits = getGrid (33, 12);
		stationery = getGrid (52, 16);*/
		christmasTree = getGrid (19, 6);//
		mappleStore = getGrid (32, 17);//
		nailSalon = getGrid (22, 2);
		holeFood = getGrid (2, 14);//
		wineSpirits = getGrid (7, 0);
		stationery = getGrid (14, 14);
	}

	private void initializeInterestPlaces(){
		interestingPlaces[0] = getGrid (0, 9);
		interestingPlaces[1] = getGrid (53, 0);
		interestingPlaces[2] = getGrid (50, 39);
		interestingPlaces[3] = getGrid (15, 18);
		interestingPlaces[4] = getGrid (42, 16);
//		interestingPlaces[5] = getGrid (, );
//		interestingPlaces[6] = getGrid (, );
//		interestingPlaces[7] = getGrid (3, 17);
//		interestingPlaces[8] = getGrid (14, 18);
//		interestingPlaces[9] = getGrid (17, 17);

	}

	public Grid getRandomPlace(){
		return interestingPlaces [UnityEngine.Random.Range (0, 5)];
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
	
	
	void readMapFromFile(){
		try
		{
			using (StreamReader sr = new StreamReader("SmallMap.txt"))
			{
				char[] b = new char[1];
				/*for (int x = height-1; x >1; --x) {
					for (int y = 0; y < width; ++y) {
						sr.Read(b, 0, 1);
						Grid g = ScriptableObject.CreateInstance<Grid>();
						g.setPointOfGrid(y, height-x+1);
						grids[y+(height-x+1)*height] = g;
						if (b[0] == '@') {
							g.gridType = Grid.GridType.UnaccessableGrid;
							GameObject cubeA = Instantiate(cube) as GameObject;
							cubeA.transform.position = new Vector3(g.pointOfGrid.y, 0.5f, g.pointOfGrid.x);
						}
						else if (b[0] == '.'){
							g.gridType = Grid.GridType.NormalGrid;
						}
					}
				}*/

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
